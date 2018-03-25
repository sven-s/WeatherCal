using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherCallFunctionsApp
{
    [Serializable]
    public class HourlyWeatherDto
    {
        public DateTime Begin { get; set; }

        public string Location { get; set; }

        public int WindSpeed { get; set; }

        public int WindBearing { get; set; }
        
        //still send exact location?
        public double Longitude { get; set; }
        public double Latitude { get; set; }



    }
}
