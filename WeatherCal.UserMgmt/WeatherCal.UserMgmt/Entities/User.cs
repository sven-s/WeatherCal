using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.UserMgmt.Entities
{
    public class User : TableEntity
    {
        public Guid Id { get; set; } = new Guid();

        public string UserName { get; set; }

        //public List<Subscription> Subscriptions { get; set; }

        public List<Feed> Feeds { get; set; }


    }
}
