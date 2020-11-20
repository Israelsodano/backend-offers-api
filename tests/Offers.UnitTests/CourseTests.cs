using System;
using System.Threading.Tasks;
using Offers.Application.Handlers;
using Offers.Domain.Commands.Course;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Responses.Course;
using Xunit;
using Offers.Domain.Commands.University;

namespace Offers.UnitTests
{
    public class CourseTests
    {
        public static async Task<Guid> CreateCourseStatic(CourseHandler CourseHandler, 
                                                          CreateCourseCommand createCourseCommand,
                                                          CampusHandler CampusHandler,
                                                          CreateCampusCommand createCampusCommand,
                                                          UniversityHandler universityHandler,
                                                          CreateUniversityCommand createUniversityCommand)
        {

            createCourseCommand.CampusId = await CampusTests.CreateCampusStatic(CampusHandler, 
                                                                                createCampusCommand, 
                                                                                universityHandler, 
                                                                                createUniversityCommand);

            createCourseCommand.Name += Guid.NewGuid();

            var result = await CourseHandler.Handle(createCourseCommand, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
            return ((CreateCourseResponse)result.Value).Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetCourse(CourseHandler handler, GetCoursesCommand command)
        {
            var result = await handler.Handle(command, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateCourse(CourseHandler CourseHandler, 
                                 CreateCourseCommand createCourseCommand,
                                 CampusHandler CampusHandler,
                                 CreateCampusCommand createCampusCommand,
                                 UniversityHandler universityHandler,
                                 CreateUniversityCommand createUniversityCommand) => CreateCourseStatic(CourseHandler, 
                                                                                                        createCourseCommand, 
                                                                                                        CampusHandler, 
                                                                                                        createCampusCommand,
                                                                                                        universityHandler,
                                                                                                        createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateCourse(CourseHandler CourseHandler, 
                                       UpdateCourseCommand updateCourseCommand, 
                                       CreateCourseCommand createCourseCommand,
                                       CampusHandler CampusHandler,
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateCourseCommand.Id = await CreateCourseStatic(CourseHandler, 
                                                              createCourseCommand,
                                                              CampusHandler,
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultUpdate = await CourseHandler.Handle(updateCourseCommand, new System.Threading.CancellationToken());
            Assert.True(resultUpdate.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteCourse(CourseHandler CourseHandler, 
                                       DeleteCourseCommand deleteCourseCommand, 
                                       CreateCourseCommand createCourseCommand,
                                       CampusHandler CampusHandler,
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
           

            deleteCourseCommand.Id = await CreateCourseStatic(CourseHandler,
                                                              createCourseCommand,
                                                              CampusHandler,
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultDelete = await CourseHandler.Handle(deleteCourseCommand, new System.Threading.CancellationToken());
            Assert.True(resultDelete.IsSuccess);
        }
    }
}
