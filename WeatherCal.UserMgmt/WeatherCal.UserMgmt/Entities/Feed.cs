using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherCal.UserMgmt.Entities
{
    public class Feed
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; }
        public List<Subscription> Subscriptions { get; set; }

    }
}
