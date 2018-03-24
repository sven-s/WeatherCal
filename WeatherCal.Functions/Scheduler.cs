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

            // get the locations from the table storage
            var locations = new List<Location>();

            // for all locations query the weather api
            var serviceTokenProvider = new AzureServiceTokenProvider();
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));
            var secretUri = SecretUri("Darksky-Key");
            var key = keyVault.GetSecretAsync(secretUri).GetAwaiter().GetResult();
            log.Info(key.Value);
            var client = new RestClient($"https://api.darksky.net/forecast/{key.Value}/37.8267,-122.4233");


            // write the response into a table storage




            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        public static string SecretUri(string secret)
        {
            return $"{WebConfigurationManager.AppSettings["KeyValutUri"]}/Secrects/{secret}";
        }

    }
}
