using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherCal.UserMgmt.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public double WindSpeedMin { get; set; }
        public double WindSpeedMax { get; set; }

        public List<(int minAngle, int maxAngle)> WindBearings { get; set; }

        //POI

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
