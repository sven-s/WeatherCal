using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using WeatherCal.AzureAPI.Models;
using WeatherCal.UserMgmt;
using WeatherCal.UserMgmt.Entities;
using System.Configuration;

namespace WeatherCal.AzureAPI.Controllers
{
    public class SubscriptionController : ApiController
    {

        public IRegistration registration = new Registration(ConfigurationManager.AppSettings["tableStorageConnectionString"]);

        // GET api/subscription
        [SwaggerOperation("GetAll")]
        public async Task<IEnumerable<Subscription>> GetAsync()
        {
            var feeds = await registration.GetFeeds();
            return feeds.SelectMany(o => o.Subscriptions);
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<Subscription> GetAsync(Guid id)
        {
            var feeds = await registration.GetFeeds();
            var subscription = feeds.SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.Id.Equals(id));
            return subscription;
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public async System.Threading.Tasks.Task<Feed> PostAsync([FromBody]SubscriptionDto subscriptionDto)
        {
            var subscription = Mapper.Map<Subscription>(subscriptionDto);
            if (!string.IsNullOrEmpty(subscriptionDto.FeedId))
            {
                return await registration.AddSubscriptionToFeed(subscription, new Guid(subscriptionDto.FeedId));
            }
            return await registration.AddSubscriptionToFeed(subscription, null);
        }

        // DELETE api/subscription/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(Guid id)
        {
            registration.DeleteSubscription(id);
        }
    }
}
