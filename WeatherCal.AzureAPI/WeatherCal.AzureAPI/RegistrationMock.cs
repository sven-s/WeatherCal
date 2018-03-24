using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WeatherCal.UserMgmt;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.AzureAPI
{
    public class RegistrationMock : IRegistration
    {
        public Task<Feed> AddSubscriptionToFeed(Subscription subscription, Guid? feedGuid)
        {
            subscription.Id = new Guid();
            var feed = new Feed()
            {
                Subscriptions = new List<Subscription>()
                {
                    subscription
                }
            };
            Task<Feed> ret = new Task<Feed>(() => feed);
            ret.RunSynchronously();
            return ret;
        }

        public void DeleteSubscription(Guid subscriptionGuid)
        {
            
        }

        public void DeleteSubscription(Subscription subscription)
        {
            
        }

        public List<Feed> GetFeeds()
        {
            return new List<Feed>();
        }

        public List<Subscription> GetSubscriptions(Guid feedGuid)
        {
            return new List<Subscription>()
            {
                new Subscription()
                {
                    Id = new Guid("43c0014e-c25e-4618-9ec1-4f8fbf01d188")
                }
            };
        }

        public List<Subscription> GetSubscriptions(Feed feed)
        {
            return new List<Subscription>()
            {
                new Subscription()
                {
                    Id = new Guid("43c0014e-c25e-4618-9ec1-4f8fbf01d188")
                }
            };
        }

        public List<Subscription> GetSubscriptions()
        {
            return new List<Subscription>()
            {
                new Subscription()
                {
                    Id = new Guid("43c0014e-c25e-4618-9ec1-4f8fbf01d188")
                }
            };
        }
    }
}