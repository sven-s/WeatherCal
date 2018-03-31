using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.FeedMgmt.Entities
{
    public class User : TableEntity
    {
        public User(Guid id)
        {
            PartitionKey = "P1";
            RowKey = id.ToString();
        }

        public User() : this(Guid.NewGuid())
        {
            
        }

        public Guid Id { get; set; } = new Guid();

        public string UserName { get; set; }

        //public List<Subscription> Subscriptions { get; set; }

        public List<Feed> Feeds { get; set; }
    }
}
