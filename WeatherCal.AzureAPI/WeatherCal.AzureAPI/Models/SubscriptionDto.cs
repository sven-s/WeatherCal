using System.Collections.Generic;

namespace WeatherCal.AzureAPI.Models
{
    public class SubscriptionDto
    {
        public string FeedId { get; set; }
        public double WindSpeedMin { get; set; }
        public double WindSpeedMax { get; set; }
        public List<(int minAngle, int maxAngle)> WindBearings { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}