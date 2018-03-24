using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using WeatherCal.AzureAPI.Models;
using WeatherCal.UserMgmt;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.AzureAPI.Controllers
{
    public class SubscriptionController : ApiController
    {

        public IRegistration registration = new RegistrationMock();

        // GET api/subscription
        [SwaggerOperation("GetAll")]
        public IEnumerable<Subscription> Get()
        {
            return registration.GetSubscriptions();
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Subscription Get(Guid id)
        {
            return registration.GetSubscriptions().Single(o => o.Id == id);
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
