using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
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

        public const string Route = "/api/v1/university";

        public UniversityTests()
        {
            Server = new ServerFixture();
            FlurlClient = new FlurlClient(Server.Client);
            FlurlClient.AllowAnyHttpStatus();
        }

        public static async Task<Guid> CreateUniversityStatic(IFlurlClient client, 
                                                              CreateUniversityCommand createUniversityCommand)
        {
            createUniversityCommand.Name += Guid.NewGuid();

            var result = await (await Route.WithClient(client)
                                           .SendJsonAsync(HttpMethod.Post, createUniversityCommand))
                                           .GetJsonAsync<CreateUniversityResponse>();

            Assert.True(result.IsSuccess);

            return result.Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetUniversities(GetUniversitiesCommand getUniversitiesCommand)
        {
            var result = await (await Route.WithClient(FlurlClient)
                                           .SendUrlEncodedAsync(HttpMethod.Get, getUniversitiesCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateUniversity(CreateUniversityCommand createUniversityCommand) => CreateUniversityStatic(FlurlClient, createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateUniversity(UpdateUniversityCommand updateUniversityCommand,
                                           CreateUniversityCommand createUniversityCommand)
        {
            updateUniversityCommand.Id = await CreateUniversityStatic(FlurlClient, createUniversityCommand);

            var result = await (await Route.WithClient(FlurlClient)
                                           .SendJsonAsync(HttpMethod.Put, updateUniversityCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteUniversity(DeleteUniversityCommand deleteUniversityCommand,
                                           CreateUniversityCommand createUniversityCommand)
        {
            deleteUniversityCommand.Id = await CreateUniversityStatic(FlurlClient, createUniversityCommand);

            var result = await Route.WithClient(FlurlClient)
                                    .SendJsonAsync(HttpMethod.Delete, deleteUniversityCommand);

            Assert.True(result.StatusCode == StatusCodes.Status204NoContent);
        }
    }
}
