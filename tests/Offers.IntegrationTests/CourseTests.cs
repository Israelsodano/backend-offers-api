using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.Course;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Course;
using Xunit;

namespace Offers.IntegrationTests
{
    public class CourseTests
    {
        public IFlurlClient FlurlClient { get; }
        public ServerFixture Server { get; set; }

        public const string Route = "/api/v1/course";

        public CourseTests()
        {
            Server = new ServerFixture();
            FlurlClient = new FlurlClient(Server.Client);
            FlurlClient.AllowAnyHttpStatus();
        }

        public static async Task<Guid> CreateCourseStatic(IFlurlClient client, 
                                                          CreateCourseCommand createCourseCommand,
                                                          CreateCampusCommand createCampusCommand,
                                                          CreateUniversityCommand createUniversityCommand)
        {
            createCourseCommand.CampusId = await CampusTests.CreateCampusStatic(client, createCampusCommand, createUniversityCommand);

            createCourseCommand.Name += Guid.NewGuid();

            var result = await (await Route.WithClient(client)
                                           .SendJsonAsync(HttpMethod.Post, createCourseCommand))
                                           .GetJsonAsync<CreateCourseResponse>();

            Assert.True(result.IsSuccess);

            return result.Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetCourses(GetCoursesCommand getCourseCommand)
        {
            var result = await (await Route.WithClient(FlurlClient)
                                           .SendUrlEncodedAsync(HttpMethod.Get, getCourseCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateCourse(CreateCourseCommand createCourseCommand,
                                 CreateCampusCommand createCampusCommand,
                                 CreateUniversityCommand createUniversityCommand) => 
            CreateCourseStatic(FlurlClient, createCourseCommand, createCampusCommand, createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateCourse(UpdateCourseCommand updateCourseCommand,
                                       CreateCourseCommand createCourseCommand,
                                       CreateCampusCommand createCampusCommand,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateCourseCommand.Id = await CreateCourseStatic(FlurlClient, createCourseCommand, createCampusCommand, createUniversityCommand);

            var result = await (await Route.WithClient(FlurlClient)
                                           .SendJsonAsync(HttpMethod.Put, updateCourseCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteCourse(DeleteCourseCommand deleteCourseCommand,
                                       CreateCourseCommand createCourseCommand,
                                       CreateCampusCommand createCampusCommand,
                                       CreateUniversityCommand createUniversityCommand)
        {
            deleteCourseCommand.Id = await CreateCourseStatic(FlurlClient, createCourseCommand, createCampusCommand, createUniversityCommand);

            var result = await Route.WithClient(FlurlClient)
                                    .SendJsonAsync(HttpMethod.Delete, deleteCourseCommand);

            Assert.True(result.StatusCode == StatusCodes.Status204NoContent);
        }
    }
}
