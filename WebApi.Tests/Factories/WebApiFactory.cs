using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;


namespace WebApi.Tests.Factories
{
    public class WebApiFactory : WebApplicationFactory<Program>
    {
        public WebApiFactory() { }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services => { });
        }
    }
}