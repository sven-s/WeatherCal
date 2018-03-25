using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.IO;

namespace WeatherCallFunctionsApp
{
    public class Functions
    {
        private static readonly log4net.ILog AIlog =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // HourlyWeatherTopicName subscription listener
        public static void WeatherAppTopicListener(
            [ServiceBusTrigger("%HourlyWeatherTopicName%", "%WeatherCalSubscriptionName%")] BrokeredMessage message, TextWriter log)
        {
            log.WriteLine("topic received v1.1");
            var hourlyWeatherDto = message.GetBody<HourlyWeatherMessage>();
            log.WriteLine("received: {0}", hourlyWeatherDto.Guid);

            //var hourlyWeather = JsonConvert.SerializeObject(hourlyWeatherDto);
        }
    }
}
