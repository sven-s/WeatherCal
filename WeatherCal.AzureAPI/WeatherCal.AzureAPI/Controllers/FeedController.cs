using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using WeatherCal.FeedMgmt;
using WeatherCal.FeedMgmt.Entities;

namespace WeatherCal.AzureAPI.Controllers
{
    public class FeedController : ApiController
    {
        private readonly IRegistration _registration = new Registration(ConfigurationData.TableStorageConnectionKey);

        [SwaggerOperation("GetAll")]
        public async System.Threading.Tasks.Task<IEnumerable<Feed>> GetAsync()
        {
            var feeds = await _registration.GetFeeds();
            return feeds;
        }

        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async System.Threading.Tasks.Task<Feed> GetAsync(Guid id)
        {
            var feeds = await _registration.GetFeeds();
            var feed = feeds.FirstOrDefault(o => o.Id.Equals(id));
            return feed;
        }
    }
}
