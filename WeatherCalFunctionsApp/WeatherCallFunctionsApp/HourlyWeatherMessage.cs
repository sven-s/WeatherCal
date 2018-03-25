using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherCal.Contracts
{
    [Serializable]
    public class HourlyWeatherMessage
    {

        public Guid SubscriptionGuid { get; set; }

        public List<HourlyWeatherDto> HourlyWeatherItems { get; set; }

    }
}
