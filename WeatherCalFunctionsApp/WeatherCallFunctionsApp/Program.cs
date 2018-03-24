using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus;
using System.Configuration;

namespace WeatherCallFunctionsApp
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        private static string _servicesBusConnectionString;
        private static NamespaceManager _namespaceManager;

        public static void Main()
        {
            _servicesBusConnectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.ServiceBus);
            _namespaceManager = NamespaceManager.CreateFromConnectionString(_servicesBusConnectionString);
            CreateTopicSubscription();

            JobHostConfiguration config = new JobHostConfiguration();
            ServiceBusConfiguration serviceBusConfig = new ServiceBusConfiguration
            {
                ConnectionString = _servicesBusConnectionString
            };
            config.UseServiceBus(serviceBusConfig);
            config.NameResolver = new QueueNameResolver();
 
            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }

        private static void CreateTopicSubscription()
        {
            var topicName = ConfigurationManager.AppSettings["HourlyWeatherTopicName"];
            var subcriptionName = ConfigurationManager.AppSettings["WeatherCalSubscriptionName"];

            if (!_namespaceManager.TopicExists(topicName))
            {
                _namespaceManager.CreateTopic(topicName);
            }

            if (!_namespaceManager.SubscriptionExists(topicName, subcriptionName))
            {
                _namespaceManager.CreateSubscription(topicName, subcriptionName);
            }
       }
    }
}
