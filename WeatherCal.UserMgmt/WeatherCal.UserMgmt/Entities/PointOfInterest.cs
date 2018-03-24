using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherCal.UserMgmt.Entities
{
    public class PointOfInterest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
