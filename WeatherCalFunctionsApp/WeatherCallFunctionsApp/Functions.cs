using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WeatherCal.Contracts;


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
            log.WriteLine("Guid received: {0}", hourlyWeatherDto.SubscriptionGuid);
            var items = hourlyWeatherDto.HourlyWeatherItems;

            var locations = items.GroupBy(h => new {h.Location, h.Begin.Day}).Select(i => i);

            foreach (var location in locations)
            {
                log.WriteLine("location received: {0}", location.Key);
            }
 
            //var hourlyWeather = JsonConvert.SerializeObject(hourlyWeatherDto);
        }
    }
}
