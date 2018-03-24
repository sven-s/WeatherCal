using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WeatherCal.AzureAPI.Models;
using WeatherCal.UserMgmt.Entities;

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
