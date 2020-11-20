using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Campus;
using Xunit;

namespace Offers.IntegrationTests
{
    public class CampusTests
    {
        public IFlurlClient FlurlClient { get; }
        public ServerFixture Server { get; set; }

        public const string Route = "/api/v1/campus";

        public CampusTests()
        {
            Server = new ServerFixture();
            FlurlClient = new FlurlClient(Server.Client);
            FlurlClient.AllowAnyHttpStatus();
        }

        public static async Task<Guid> CreateCampusStatic(IFlurlClient client, 
                                                          CreateCampusCommand createCampusCommand,
                                                          CreateUniversityCommand createUniversityCommand)
        {
            createCampusCommand.UniversityId = await UniversityTests.CreateUniversityStatic(client, createUniversityCommand);

            createCampusCommand.Name += Guid.NewGuid();

            var result = await (await Route.WithClient(client)
                                           .SendJsonAsync(HttpMethod.Post, createCampusCommand))
                                           .GetJsonAsync<CreateCampusResponse>();

            Assert.True(result.IsSuccess);

            return result.Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetCampus(GetCampusCommand getCampusCommand)
        {
            var result = await (await Route.WithClient(FlurlClient)
                                           .SendUrlEncodedAsync(HttpMethod.Get, getCampusCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateCampus(CreateCampusCommand createCampusCommand, CreateUniversityCommand createUniversityCommand) => 
            CreateCampusStatic(FlurlClient, createCampusCommand, createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateCampus(UpdateCampusCommand updateCampusCommand,
                                       CreateCampusCommand createCampusCommand,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateCampusCommand.Id = await CreateCampusStatic(FlurlClient, createCampusCommand, createUniversityCommand);

            var result = await (await Route.WithClient(FlurlClient)
                                           .SendJsonAsync(HttpMethod.Put, updateCampusCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteCampus(DeleteCampusCommand deleteCampusCommand,
                                       CreateCampusCommand createCampusCommand, 
                                       CreateUniversityCommand createUniversityCommand)
        {
            deleteCampusCommand.Id = await CreateCampusStatic(FlurlClient, createCampusCommand, createUniversityCommand);

            var result = await Route.WithClient(FlurlClient)
                                    .SendJsonAsync(HttpMethod.Delete, deleteCampusCommand);

            Assert.True(result.StatusCode == StatusCodes.Status204NoContent);
        }
    }
}
