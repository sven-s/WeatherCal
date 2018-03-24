using System;
using System.Collections.Generic;
using System.Web.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using RestSharp;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace WeatherCal.Functions
{
    public static class Scheduler
    {
        struct Location
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }        

        [FunctionName("Scheduler")]
        public static void Run([TimerTrigger("0 */30 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Starting scheduler at: {DateTime.Now}");
            // get the locations from the table storage
            var locations = new List<Location>();

            // for all locations query the weather api

            try
            {
                var serviceTokenProvider = new AzureServiceTokenProvider();
                var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));
                var secretUri = SecretUri("Darksky-Key");
                var key = keyVault.GetSecretAsync(secretUri).GetAwaiter().GetResult();
                var client = new RestClient("https://api.darksky.net");

                var request = new RestRequest($"forecast/{key.Value}/37.8267,-122.4233", Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string
                log.Info(content);

            }
            catch (Exception ex)
            {
                log.Error("Cannot retrieve key", ex);
                throw;
            }

            // write the response into a table storage


            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        public static string SecretUri(string secret)
        {
            return $"{WebConfigurationManager.AppSettings["KeyVaultUri"]}Secrets/{secret}";
        }

    }
}
