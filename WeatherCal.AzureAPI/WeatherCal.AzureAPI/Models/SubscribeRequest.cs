using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherCal.AzureAPI.Models
{
    public class SubscribeRequest
    {
        public string Name { get; set; }

        public decimal Longitute { get; set; }

        public decimal Latitute { get; set; }

        public decimal MinimalWindSpeed { get; set; }

        public decimal MaximalWindSpeed { get; set; }

        public List<(int minWindBearing, int maxWindBearing)> WindBearings { get; set; }

    }
}