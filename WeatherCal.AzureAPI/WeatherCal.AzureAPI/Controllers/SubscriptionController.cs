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
        // GET api/subscription
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(Guid id)
        {
            return "value";
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public void Post([FromBody]SubscriptionDto subscriptionDto)
        {
            var subscription = Mapper.Map<Subscription>(subscriptionDto);
            var registration = new Registration();
            registration.CreateSubscription(subscription, new Guid(subscriptionDto.FeedId));
        }

        // PUT api/subscription/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(Guid id, [FromBody]string value)
        {
            
        }

        // DELETE api/subscription/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(Guid id)
        {

        }
    }
}
