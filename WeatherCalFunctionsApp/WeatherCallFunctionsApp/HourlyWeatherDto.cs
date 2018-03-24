using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCallFunctionsApp
{
    public class HourlyWeatherDto
    {
        public DateTime DateTimeFrom { get; set; }
        public string Location { get; set; }
        public float WindSpeed { get; set; }
        public int WindDirection { get; set; }
    }
}
