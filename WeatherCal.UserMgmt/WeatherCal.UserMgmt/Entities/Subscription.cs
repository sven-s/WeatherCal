using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.UserMgmt.Entities
{
    public class Subscription : TableEntity
    {
        public Subscription(Guid id)
        {
            PartitionKey = "P1";
            RowKey = id.ToString();
            WindBearings = new List<(int minAngle, int maxAngle)>();
        }

        public Subscription() : this(Guid.NewGuid())
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid FeedGuid { get; set; }

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
