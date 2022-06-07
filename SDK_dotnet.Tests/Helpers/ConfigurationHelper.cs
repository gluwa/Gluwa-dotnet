using Microsoft.Extensions.Configuration;
namespace SDK_dotnet.Tests.Helpers
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot GetIConfigurationRoot() 
            => new ConfigurationBuilder()
                .AddUserSecrets("fb2e4e07-ec86-41f4-adac-b07460a68502")
                .Build();

        public static string GetByKey(string key) =>  GetIConfigurationRoot().GetSection(key).Value;
    }
}