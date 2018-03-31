using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using WeatherCal.AzureAPI.Models;
using WeatherCal.FeedMgmt;
using WeatherCal.FeedMgmt.Entities;

namespace WeatherCal.AzureAPI.Controllers
{
    public class SubscriptionController : ApiController
    {
        public IRegistration Registration = new Registration(ConfigurationData.TableStorageConnectionKey);

        // GET api/subscription
        [SwaggerOperation("GetAll")]
        public async Task<IEnumerable<Subscription>> GetAsync()
        {
            var feeds = await Registration.GetFeeds();
            return feeds.SelectMany(o => o.Subscriptions);
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<Subscription> GetAsync(Guid id)
        {
            var feeds = await Registration.GetFeeds();
            var subscription = feeds.SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.Id.Equals(id));
            return subscription;
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public async Task<Feed> PostAsync([FromBody]SubscriptionDto subscriptionDto)
        {
            var subscription = Mapper.Map<Subscription>(subscriptionDto);
            if (!string.IsNullOrEmpty(subscriptionDto.FeedId))
            {
                return await Registration.AddSubscriptionToFeed(subscription, new Guid(subscriptionDto.FeedId));
            }
            return await Registration.AddSubscriptionToFeed(subscription, null);
        }

        // DELETE api/subscription/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(Guid id)
        {
            Registration.DeleteSubscription(id);
        }
    }
}
