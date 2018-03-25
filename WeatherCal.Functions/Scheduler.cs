using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using RestSharp;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Message = Microsoft.Azure.ServiceBus.Message;
using TopicClient = Microsoft.Azure.ServiceBus.TopicClient;

namespace WeatherCal.Functions
{
    public static class Scheduler
    {


        public class Rootobject
        {
            public float latitude { get; set; }
            public float longitude { get; set; }
            public string timezone { get; set; }
            public Hourly hourly { get; set; }
            public int offset { get; set; }
        }

        public class Hourly
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public Datum[] data { get; set; }
        }

        public class Datum
        {
            public int time { get; set; }
            public string summary { get; set; }
            public string icon { get; set; }
            public float precipIntensity { get; set; }
            public float precipProbability { get; set; }
            public float temperature { get; set; }
            public float apparentTemperature { get; set; }
            public float dewPoint { get; set; }
            public float humidity { get; set; }
            public float pressure { get; set; }
            public float windSpeed { get; set; }
            public float windGust { get; set; }
            public int windBearing { get; set; }
            public float cloudCover { get; set; }
            public int uvIndex { get; set; }
            public float visibility { get; set; }
            public float ozone { get; set; }
            public string precipType { get; set; }
            public float precipAccumulation { get; set; }
        }





        [FunctionName("Scheduler")]
        public static void Run([TimerTrigger("0 */30 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Starting scheduler at: {DateTime.Now}");
            // get the locations from the table storage
            var locations = new List<(double lat, double lng)>();
            locations.Add((lat: 54.2906577, lng: 8.5807452));

            // for all locations query the weather api
            try
            {
                var serviceTokenProvider = new AzureServiceTokenProvider();
                var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));
                var secretUri = SecretUri("Darksky-Key");
                var key = keyVault.GetSecretAsync(secretUri).GetAwaiter().GetResult();
                var client = new RestClient("https://api.darksky.net");

                foreach (var location in locations)
                {
                    var request = new RestRequest($"forecast/{key.Value}/{location.lat},{location.lng}?exclude=currently,daily,flags&lang=de&units=si", Method.GET);
//                    IRestResponse response = client.Execute(request);
  
                    var response2 = client.Execute<Rootobject>(request);
                    //var content = response2.Data.timezone; // raw content as string
                    //log.Info(content);

                    // do some magic filtering 


                    // push the stuff into the service bus

                    //var responseMessage = new BrokeredMessage() { Guid = guid, Location = location };
                    //AzureServiceBusManager.SendResponseMessage(responseMessage);

                    string connectionString = "Endpoint=sb://weathercal.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=b9WBGHjtpJjvowzIuqbeDCD3fI7ii7h2aLvE3N4BKPI=";
 
                    MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);
 
//Sending a message


                    var productRating = new { ProductId = 123, RatingSum = 23 };
                    var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(productRating)));

                    var topicClient = new TopicClient(connectionString, "hourlyweather");
                    topicClient.SendAsync(message).GetAwaiter().GetResult();


                }


            }
            catch (Exception ex)
            {
                log.Error("Error during execution", ex);
                throw;
            }

            // write the response into the message bus


            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        public static string SecretUri(string secret)
        {
            return $"{WebConfigurationManager.AppSettings["KeyVaultUri"]}Secrets/{secret}";
        }

    }
}
