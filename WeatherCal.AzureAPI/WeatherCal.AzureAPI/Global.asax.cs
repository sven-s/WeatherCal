using AutoMapper;
using System.Web.Http;
using WeatherCal.AzureAPI.Models;
using WeatherCal.FeedMgmt.Entities;

namespace WeatherCal.AzureAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Mapper.Initialize(cfg => cfg.CreateMap<SubscriptionDto, Subscription>());
        }
    }
}
