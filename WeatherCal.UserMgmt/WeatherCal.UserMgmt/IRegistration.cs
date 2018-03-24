using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.UserMgmt
{
    public interface IRegistration
    {
        Task<Feed> AddSubscriptionToFeed(Subscription subscription, Guid? feedGuid);
        void DeleteSubscription(Guid subscriptionGuid);
        void DeleteSubscription(Subscription subscription);
        Task<List<Feed>> GetFeeds();
        Task<List<Subscription>> GetSubscriptions(Guid feedGuid);
        Task<List<Subscription>> GetSubscriptions(Feed feed);
        //Task<List<Subscription>> GetSubscriptions();
    }
}
