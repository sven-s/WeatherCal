using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherCal.UserMgmt;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.AzureAPI.Controllers
{
    public class FeedController : ApiController
    {
        IRegistration registration = new RegistrationMock();
        [SwaggerOperation("GetAll")]
        public async System.Threading.Tasks.Task<IEnumerable<Feed>> GetAsync()
        {
            var feeds = await registration.GetFeeds();
            return feeds;
        }

        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async System.Threading.Tasks.Task<Feed> GetAsync(Guid id)
        {
            var feeds = await registration.GetFeeds();
            var feed = feeds.FirstOrDefault(o => o.Id.Equals(id));
            return feed;
        }
    }
}
