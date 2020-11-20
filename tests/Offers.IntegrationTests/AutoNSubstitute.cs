using System;
using AutoFixture;
using AutoFixture.Xunit2;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.Course;
using Offers.Domain.Commands.Offer;
using Offers.Domain.Commands.University;

namespace Offers.IntegrationTests
{
    public class AutoNSubstitute : AutoDataAttribute
    {
        public AutoNSubstitute(Func<IFixture> func) : base(func)
        {

        }

        public AutoNSubstitute() : this(FixtureFactory)
        {

        }


        public static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
           
            fixture.Register(() => new CreateUniversityCommand() { LogoUrl = "test", Name = "Test", Score = 5 });
            fixture.Register(() => new UpdateUniversityCommand() { Id = Guid.Empty, LogoUrl = "Test", Name = "Test", Score = 5 });
            fixture.Register(() => new DeleteUniversityCommand() { Id = Guid.Empty });
            fixture.Register(() => new GetUniversitiesCommand());

            fixture.Register(() => new CreateCampusCommand() { City = "test", UniversityId = Guid.Empty, Name = "test" });
            fixture.Register(() => new UpdateCampusCommand() { City = "test", UniversityId = Guid.Empty, Name = "test", Id = Guid.Empty });
            fixture.Register(() => new DeleteCampusCommand() { Id = Guid.Empty });
            fixture.Register(() => new GetCampusCommand());

            fixture.Register(() => new CreateCourseCommand() { Name = "test" });
            fixture.Register(() => new UpdateCourseCommand() { Name = "test", Id = Guid.Empty, CampusId = Guid.Empty });
            fixture.Register(() => new DeleteCourseCommand() { Id = Guid.Empty });
            fixture.Register(() => new GetCoursesCommand());

            fixture.Register(() => new CreateOfferCommand() { CourseId = Guid.Empty, DiscountPercentage = 50, FullPrice = 1000, EnrollmentSemesterYear = 2020, Semester = Domain.Entities.Enums.Semester.Second, StartDate = DateTime.Now });
            fixture.Register(() => new UpdateOfferCommand() { DiscountPercentage = 50, FullPrice = 1000, EnrollmentSemesterYear = 2020, Semester = Domain.Entities.Enums.Semester.Second, StartDate = DateTime.Now, Enabled = false, Id = Guid.Empty });
            fixture.Register(() => new DeleteCourseCommand() { Id = Guid.Empty });
            fixture.Register(() => new GetOffersCommand());

            return fixture;
        }
    }
}
