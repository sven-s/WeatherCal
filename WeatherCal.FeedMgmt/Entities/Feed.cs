using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.FeedMgmt.Entities
{
    public class Feed : TableEntity
    {
        public Feed() : this(Guid.NewGuid())
        {
        }

        public Feed(Guid id)
        {
            PartitionKey = "P1";
            RowKey = id.ToString();
            Subscriptions = new List<Subscription>();
        }

        public Guid Id => Guid.Parse(RowKey);
        public User User { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
