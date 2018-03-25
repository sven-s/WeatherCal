using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using System.Linq;
using WeatherCal.Contracts;

//using Ical.Net;
//using Ical.Net.CalendarComponents;
//using Ical.Net.DataTypes;

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

            var locations = items.GroupBy(h => h.Location).Select(i => i);
            foreach (var location in locations)
            {
                log.WriteLine("location received: {0}", location.Key);

            }



            //var calendar = new Calendar();
            //calendar.AddProperty("X-WR-CALNAME", "SurfWeather"); // sets the calendar title
            //calendar.AddProperty("X-ORIGINAL-URL", "http://aka.ms/AzureFunctionsLive");
            //calendar.AddProperty("METHOD", "PUBLISH");
            
            //var icalevent = new CalendarEvent()
            //{
            //    DtStart = new CalDateTime(new DateTime(2017, 07, 06, 18, 0, 0, DateTimeKind.Utc)),
            //    DtEnd = new CalDateTime(new DateTime(2017, 07, 06, 19, 0, 0, DateTimeKind.Utc)),
            //    Created = new CalDateTime(DateTime.Now),
            //    Location = "http://aka.ms/AzureFunctionsLive",
            //    Summary = "Azure Function Webinar",
            //    Url = new Uri("http://aka.ms/AzureFunctionsLive")
            //};

            //var hourlyWeather = JsonConvert.SerializeObject(hourlyWeatherDto);
        }
    }
}
