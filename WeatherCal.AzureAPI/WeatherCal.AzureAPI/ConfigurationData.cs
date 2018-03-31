using System.Web.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace WeatherCal.AzureAPI
{
    public static class ConfigurationData
    {
        private static string _connectionString;

        public static string TableStorageConnectionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionString)) return _connectionString;
                
                var serviceTokenProvider = new AzureServiceTokenProvider();
                var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));
                var subscriptionTableUri = SecretUri("Subscription-Table");
                var tableStorageConnectionKey = keyVault.GetSecretAsync(subscriptionTableUri).GetAwaiter().GetResult();

                return tableStorageConnectionKey.Value;
            }
        }
        static string SecretUri(string secret)
        {
            return $"{WebConfigurationManager.AppSettings["KeyVaultUri"]}Secrets/{secret}";
        }
    }
}