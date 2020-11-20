using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.University;
using Xunit;

namespace Offers.IntegrationTests
{
    public class UniversityTests
    {
        public IFlurlClient FlurlClient { get; }
        public ServerFixture Server { get; set; }
        public string Route { get; set; }

        public UniversityTests()
        {
            Server = new ServerFixture();
            FlurlClient = new FlurlClient(Server.Client);
            FlurlClient.AllowAnyHttpStatus();
            Route = "/api/v1/university";
        }

        [Theory, AutoNSubstitute]
        public async Task GetUniversities(GetUniversitiesCommand getUniversitiesCommand)
        {
            var result = await (await Route.WithClient(FlurlClient)
                                           .SendUrlEncodedAsync(HttpMethod.Get, getUniversitiesCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }
    }
}
