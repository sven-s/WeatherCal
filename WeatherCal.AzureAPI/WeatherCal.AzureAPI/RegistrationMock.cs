using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherCal.FeedMgmt;
using WeatherCal.FeedMgmt.Entities;

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
            var ret = new Task<Feed>(() => feed);
            ret.RunSynchronously();
            return ret;
        }

        public void DeleteSubscription(Guid subscriptionGuid)
        {
            
        }

        public void DeleteSubscription(Subscription subscription)
        {
            
        }

        public Task<List<Feed>> GetFeeds()
        {
            var feeds = new List<Feed>();
            var ret = new Task<List<Feed>>(() => feeds);
            ret.RunSynchronously();
            return ret;
        }

        public Task<List<Subscription>> GetSubscriptions(Guid feedGuid)
        {
            var subscriptions = new List<Subscription>()
            {
                new Subscription()
                {
                    Id = new Guid("43c0014e-c25e-4618-9ec1-4f8fbf01d188")
                }
            };
            var ret = new Task<List<Subscription>>(() => subscriptions);
            ret.RunSynchronously();
            return ret;
        }

        public Task<List<Subscription>> GetSubscriptions(Feed feed)
        {
            var subscriptions = new List<Subscription>()
            {
                new Subscription()
                {
                    Id = new Guid("43c0014e-c25e-4618-9ec1-4f8fbf01d188")
                }
            };
            var ret = new Task<List<Subscription>>(() => subscriptions);
            ret.RunSynchronously();
            return ret;
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