using System;
using System.Threading.Tasks;
using Offers.Application.Handlers;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.Campus;
using Xunit;

namespace Offers.UnitTests
{
    public class CampusTests
    {
        public static async Task<Guid> CreateCampusStatic(CampusHandler campusHandler, 
                                                          CreateCampusCommand createCampusCommand,
                                                          UniversityHandler universityHandler,
                                                          CreateUniversityCommand createUniversityCommand)
        {

            createCampusCommand.UniversityId = await UniversityTests.CreateUniversityStatic(universityHandler, createUniversityCommand);
            createCampusCommand.Name += Guid.NewGuid();

            var result = await campusHandler.Handle(createCampusCommand, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
            return ((CreateCampusResponse)result.Value).Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetCampus(CampusHandler handler, GetCampusCommand command)
        {
            var result = await handler.Handle(command, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateCampus(CampusHandler campusHandler, 
                                 CreateCampusCommand createCampusCommand,
                                 UniversityHandler universityHandler,
                                 CreateUniversityCommand createUniversityCommand) => CreateCampusStatic(campusHandler, 
                                                                                                        createCampusCommand, 
                                                                                                        universityHandler, 
                                                                                                        createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateCampus(CampusHandler campusHandler, 
                                       UpdateCampusCommand updateCampusCommand, 
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateCampusCommand.Id = await CreateCampusStatic(campusHandler, 
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultUpdate = await campusHandler.Handle(updateCampusCommand, new System.Threading.CancellationToken());
            Assert.True(resultUpdate.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteCampus(CampusHandler campusHandler, 
                                       DeleteCampusCommand deleteCampusCommand, 
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
           

            deleteCampusCommand.Id = await CreateCampusStatic(campusHandler,
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultDelete = await campusHandler.Handle(deleteCampusCommand, new System.Threading.CancellationToken());
            Assert.True(resultDelete.IsSuccess);
        }
    }
}
