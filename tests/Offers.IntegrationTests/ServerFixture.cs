using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.Api;
using Offers.Application.Contexts;

namespace Offers.IntegrationTests
{
    public class ServerFixture
    {
        public HttpClient Client { get; }
        public ServerFixture()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Test")
                    .ConfigureAppConfiguration((hosting, config) => { config.AddJsonFile("appsettings.Development.json"); })
                    .UseStartup<Startup>()
                    .ConfigureServices(services => 
                        services.AddSingleton(x => new DbContextOptionsBuilder<OffersContext>().UseInMemoryDatabase("OffersTest").Options)
                    ));

            Client = server.CreateClient();
        }
    }
}
