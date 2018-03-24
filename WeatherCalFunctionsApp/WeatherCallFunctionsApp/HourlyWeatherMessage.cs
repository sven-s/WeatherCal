using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCallFunctionsApp
{
    public class HourlyWeatherMessage
    {
        public string Guid { get; set; }

        public List<HourlyWeatherDto> HourlyWeatherItems { get; set; }
    }
}
