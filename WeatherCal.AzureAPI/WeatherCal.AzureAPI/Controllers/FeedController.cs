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
        public IEnumerable<Feed> Get()
        {
            return registration.GetFeeds();
        }

        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Feed Get(Guid id)
        {
            return registration.GetFeeds().Single(o => o.Id == id);
        }
    }
}
