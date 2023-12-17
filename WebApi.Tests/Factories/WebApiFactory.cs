using WebApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;


namespace WebApi.Tests.Factories
{
    public class WebApiFactory : WebApplicationFactory<Program>
    {

        public string[] ApiKeyArray;
        public readonly string InvalidApiKey = AppHelper.GenerateNuGuid();

        public WebApiFactory()
        {
            var apiKeys = "testing;development";
            Environment.SetEnvironmentVariable(Constant.API_KEY, apiKeys);
            ApiKeyArray = apiKeys.Split(';');
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services => { });
        }
    }
}