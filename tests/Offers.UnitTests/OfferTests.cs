using System;
using System.Threading.Tasks;
using Offers.Application.Handlers;
using Offers.Domain.Commands.Offer;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Responses.Offer;
using Xunit;
using Offers.Domain.Commands.University;
using Offers.Domain.Commands.Course;

namespace Offers.UnitTests
{
    public class OfferTests
    {
        public static async Task<Guid> CreateOfferStatic(OfferHandler OfferHandler, 
                                                         CreateOfferCommand createOfferCommand,
                                                         CourseHandler courseHandler,
                                                         CreateCourseCommand createCourseCommand,
                                                         CampusHandler campusHandler,
                                                         CreateCampusCommand createCampusCommand,
                                                         UniversityHandler universityHandler,
                                                         CreateUniversityCommand createUniversityCommand)
        {

            createOfferCommand.CourseId = await CourseTests.CreateCourseStatic(courseHandler, 
                                                                               createCourseCommand,
                                                                               campusHandler, 
                                                                               createCampusCommand, 
                                                                               universityHandler, 
                                                                               createUniversityCommand);
            createOfferCommand.DiscountPercentage = new Random().Next(0, 100);

            var result = await OfferHandler.Handle(createOfferCommand, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
            return ((CreateOfferResponse)result.Value).Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetOffer(OfferHandler handler, GetOffersCommand command)
        {
            var result = await handler.Handle(command, new System.Threading.CancellationToken());
            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateOffer(OfferHandler OfferHandler, 
                                 CreateOfferCommand createOfferCommand,
                                 CourseHandler courseHandler,
                                 CreateCourseCommand createCourseCommand,
                                 CampusHandler campusHandler,
                                 CreateCampusCommand createCampusCommand,
                                 UniversityHandler universityHandler,
                                 CreateUniversityCommand createUniversityCommand) => CreateOfferStatic(OfferHandler, 
                                                                                                        createOfferCommand,
                                                                                                        courseHandler,
                                                                                                        createCourseCommand,
                                                                                                        campusHandler, 
                                                                                                        createCampusCommand,
                                                                                                        universityHandler,
                                                                                                        createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateOffer(OfferHandler OfferHandler, 
                                       UpdateOfferCommand updateOfferCommand, 
                                       CreateOfferCommand createOfferCommand,
                                       CourseHandler courseHandler,
                                       CreateCourseCommand createCourseCommand,
                                       CampusHandler campusHandler,
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateOfferCommand.Id = await CreateOfferStatic(OfferHandler, 
                                                              createOfferCommand,
                                                              courseHandler,
                                                              createCourseCommand,
                                                              campusHandler,
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultUpdate = await OfferHandler.Handle(updateOfferCommand, new System.Threading.CancellationToken());
            Assert.True(resultUpdate.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteOffer(OfferHandler OfferHandler, 
                                       DeleteOfferCommand deleteOfferCommand, 
                                       CreateOfferCommand createOfferCommand,
                                       CourseHandler courseHandler,
                                       CreateCourseCommand createCourseCommand,
                                       CampusHandler campusHandler,
                                       CreateCampusCommand createCampusCommand,
                                       UniversityHandler universityHandler,
                                       CreateUniversityCommand createUniversityCommand)
        {
           

            deleteOfferCommand.Id = await CreateOfferStatic(OfferHandler,
                                                              createOfferCommand,
                                                              courseHandler,
                                                              createCourseCommand,
                                                              campusHandler,
                                                              createCampusCommand,
                                                              universityHandler,
                                                              createUniversityCommand);

            var resultDelete = await OfferHandler.Handle(deleteOfferCommand, new System.Threading.CancellationToken());
            Assert.True(resultDelete.IsSuccess);
        }
    }
}
