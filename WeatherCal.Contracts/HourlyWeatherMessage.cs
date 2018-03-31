using System;
using System.Collections.Generic;

namespace WeatherCal.Contracts
{
    [Serializable]
    public class HourlyWeatherMessage
    {
        public Guid SubscriptionGuid { get; set; }

        public List<HourlyWeatherDto> HourlyWeatherItems { get; set; }
    }
}
