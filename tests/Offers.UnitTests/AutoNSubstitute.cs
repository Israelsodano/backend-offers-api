using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Common.Repository;
using Common.Repository.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Offers.Api;
using Offers.Api.Controllers;
using Offers.Application.Contexts;
using Offers.Application.Extensions;
using Offers.Application.Handlers;
using Offers.Application.Pipelines;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.Course;
using Offers.Domain.Commands.Offer;
using Offers.Domain.Commands.University;
using Offers.Domain.Entities;

namespace Offers.UnitTests
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
            var services = new ServiceCollection();

            services.AddValidators();
            services.AddApplicationServices();
            
            services.AddTransient<UniversityHandler>();
            services.AddTransient<CampusHandler>();
            services.AddTransient<CourseHandler>();
            services.AddTransient<OfferHandler>();

            services.AddSingleton(x => new DbContextOptionsBuilder<OffersContext>().UseInMemoryDatabase("OffersTest").Options);
            services.AddRepositoryServices(cfg => {
                cfg.AddUnit<University, OffersContext>();
                cfg.AddUnit<Campus, OffersContext>();
                cfg.AddUnit<Course, OffersContext>();
                cfg.AddUnit<Offer, OffersContext>();
            });

            var provider = services.BuildServiceProvider();
          
            fixture.Register(() => provider.GetService<UniversityHandler>());
            fixture.Register(() => provider.GetService<CampusHandler>());
            fixture.Register(() => provider.GetService<CourseHandler>());
            fixture.Register(() => provider.GetService<OfferHandler>());

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
