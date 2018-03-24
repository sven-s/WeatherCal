using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.UserMgmt.Entities
{
    public class Feed : TableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; }
        public List<Subscription> Subscriptions { get; set; }

    }
}
