using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeatherCal.WebClient.Startup))]
namespace WeatherCal.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
