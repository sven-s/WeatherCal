using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.UserMgmt.Entities
{
    public class Feed : TableEntity
    {
        public Feed() : this(Guid.NewGuid())
        {
        }

        public Feed(Guid id)
        {
            RowKey = id.ToString();
        }

        public Guid Id => Guid.Parse(RowKey);
        public User User { get; set; }
        public List<Subscription> Subscriptions { get; set; }

    }
}
