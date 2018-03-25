using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using RestSharp;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using WeatherCal.Contracts;
using WeatherCal.UserMgmt;

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
            public List<WeatherData> data { get; set; }
        }

        public class WeatherData
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
            var locations = new List<(string name, double lat, double lng)>();
            locations.Add((name: "St. Peter Ording", lat: 54.2906577, lng: 8.5807452));
            locations.Add((name: "Klitmoeller",lat: 57.026376, lng: 8.4780126));
            locations.Add((name: "Heiligenhafen",lat: 54.3843142, lng: 10.9413962));
            
            var m = new HourlyWeatherMessage();
            m.SubscriptionGuid = Guid.NewGuid();
            m.HourlyWeatherItems = new List<HourlyWeatherDto>();

            // for all locations query the weather api
            try
            {
                var serviceTokenProvider = new AzureServiceTokenProvider();
                var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));

                var secretUri = SecretUri("Darksky-Key");
                var key = keyVault.GetSecretAsync(secretUri).GetAwaiter().GetResult();
                var client = new RestClient("https://api.darksky.net");


                var subscriptionTableUri = SecretUri("Subscription-Table");
                var tableStorageConnectionKey = keyVault.GetSecretAsync(subscriptionTableUri).GetAwaiter().GetResult();

                var instance = new Registration(tableStorageConnectionKey.Value);
                var feeds = instance.GetFeeds().GetAwaiter().GetResult();

                foreach (var feed in feeds)
                {
                    foreach (var subscription in feed.Subscriptions)
                    {
                        log.Info($"FeedId {feed.Id} subscription:{subscription.Name} lat:{subscription.Latitude} lng:{subscription.Longitude} min:{subscription.WindSpeedMin} max:{subscription.WindSpeedMax} ");

                        log.Info($"Get weather data for {subscription.Name} {subscription.Longitude} {subscription.Longitude}");

                        var request = new RestRequest($"forecast/{key.Value}/{subscription.Longitude},{subscription.Longitude}?exclude=currently,daily,flags&lang=de&units=si&extend=hourly", Method.GET); 
                        var response = client.Execute<Rootobject>(request);
                        log.Info($"Got response from dark sky");
                        
                        foreach (var bearing in subscription.WindBearings)
                        {
                            log.Info($"  Bearing min:{bearing.minAngle} max:{bearing.maxAngle}");
                        }

                        // do some magic filtering 
                        var filtereditems = response.Data.hourly.data
                            .Where(x => x.windSpeed >= subscription.WindSpeedMin && x.windSpeed <= subscription.WindSpeedMax)
                            .ToList();

                        foreach (var bearing in subscription.WindBearings)
                        {
                            filtereditems = filtereditems.Where(x =>
                                x.windBearing >= bearing.maxAngle && x.windBearing <= bearing.maxAngle).ToList();
                        }

                        log.Info($"Got {filtereditems.Count} filtered items for {subscription.Name} from {response.Data.hourly.data.Count} total items");

                        m.HourlyWeatherItems.AddRange(filtereditems
                            .Select(x => new HourlyWeatherDto
                            {
                                Location = subscription.Name,
                                Begin = UnixTimeStampToDateTime (x.time),
                                WindBearing = x.windBearing,
                                WindSpeed = (int)x.windSpeed
                            }));
                    }
                }

                foreach (var location in locations)
                {
                    log.Info($"Get weather data for {location.name} {location.lat} {location.lng}");

                    var request = new RestRequest($"forecast/{key.Value}/{location.lat},{location.lng}?exclude=currently,daily,flags&lang=de&units=si&extend=hourly", Method.GET); 
                    var response = client.Execute<Rootobject>(request);
                    log.Info($"Got response from dark sky");

                    // do some magic filtering 
                    var filtereditems = response.Data.hourly.data.Where(x => x.windSpeed > 7 && x.windBearing > 180)
                        .ToList();
                    log.Info($"Got {filtereditems.Count} filtered items for {location.name} from {response.Data.hourly.data.Count} total items");

                    m.HourlyWeatherItems.AddRange(filtereditems
                        .Select(x => new HourlyWeatherDto
                        {
                            Location = location.name,
                            Begin = UnixTimeStampToDateTime (x.time),
                            WindBearing = x.windBearing,
                            WindSpeed = (int)x.windSpeed
                        }));

                }
                // push the stuff into the service bus
                log.Info($"Having {m.HourlyWeatherItems.Count} items");
                
                ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;
                string connectionString = "Endpoint=sb://weathercal.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=b9WBGHjtpJjvowzIuqbeDCD3fI7ii7h2aLvE3N4BKPI=";

                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);                   
                var messagingFactory = MessagingFactory.Create(namespaceManager.Address,namespaceManager.Settings.TokenProvider);
                var myclient = messagingFactory.CreateTopicClient("hourlyweather");                 
                var bm = new BrokeredMessage(m);
                myclient.Send(bm);

            }
            catch (Exception ex)
            {
                log.Error("Error during execution", ex);
                throw;
            }

            // write the response into the message bus


            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        static string SecretUri(string secret)
        {
            return $"{WebConfigurationManager.AppSettings["KeyVaultUri"]}Secrets/{secret}";
        }
    }
}
