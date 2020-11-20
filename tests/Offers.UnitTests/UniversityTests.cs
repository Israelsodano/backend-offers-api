using System;
using System.Threading.Tasks;
using Common.Result;
using Offers.Application.Handlers;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.University;
using Xunit;

namespace Offers.UnitTests
{
    public class UniversityTests
    {
        public static async Task<Guid> CreateUniversityStatic(UniversityHandler handler, CreateUniversityCommand command)
        {
            command.Name += Guid.NewGuid();
            var result = await handler.Handle(command, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
            return ((CreateUniversityResponse)result.Value).Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetUniversities(UniversityHandler handler, GetUniversitiesCommand command)
        {
            var result = await handler.Handle(command, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateUniversity(UniversityHandler handler, CreateUniversityCommand command) => CreateUniversityStatic(handler, command);

        [Theory, AutoNSubstitute]
        public async Task UpdateUniversity(UniversityHandler handler, UpdateUniversityCommand updateCommand, CreateUniversityCommand createCommand)
        {


            updateCommand.Id = await CreateUniversityStatic(handler, createCommand);
            var resultUpdate = await handler.Handle(updateCommand, new System.Threading.CancellationToken());
            Assert.True(resultUpdate.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteUniversity(UniversityHandler handler, DeleteUniversityCommand deleteCommand, CreateUniversityCommand createCommand)
        {
            deleteCommand.Id = await CreateUniversityStatic(handler, createCommand);
            var resultDelete = await handler.Handle(deleteCommand, new System.Threading.CancellationToken());
            Assert.True(resultDelete.IsSuccess);
        }
    }
}
