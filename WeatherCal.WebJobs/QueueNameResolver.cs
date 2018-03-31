using Microsoft.Azure.WebJobs;
using System.Configuration;

namespace WeatherCallFunctionsApp
{
    public class QueueNameResolver : INameResolver
    {
        public string Resolve(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
