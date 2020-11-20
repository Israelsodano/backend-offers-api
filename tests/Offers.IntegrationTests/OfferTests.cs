using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Commands.Course;
using Offers.Domain.Commands.Offer;
using Offers.Domain.Commands.University;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Offer;
using Xunit;

namespace Offers.IntegrationTests
{
    public class OfferTests
    {
        public IFlurlClient FlurlClient { get; }
        public ServerFixture Server { get; set; }

        public const string Route = "/api/v1/offer";

        public OfferTests()
        {
            Server = new ServerFixture();
            FlurlClient = new FlurlClient(Server.Client);
            FlurlClient.AllowAnyHttpStatus();
        }

        public static async Task<Guid> CreateOfferStatic(IFlurlClient client, 
                                                          CreateOfferCommand createOfferCommand,
                                                          CreateCourseCommand createCourseCommand,
                                                          CreateCampusCommand createCampusCommand,
                                                          CreateUniversityCommand createUniversityCommand)
        {
            createOfferCommand.CourseId = await CourseTests.CreateCourseStatic(client, createCourseCommand, createCampusCommand, createUniversityCommand);

            createOfferCommand.DiscountPercentage = new Random().Next(0, 100);

            var result = await (await Route.WithClient(client)
                                           .SendJsonAsync(HttpMethod.Post, createOfferCommand))
                                           .GetJsonAsync<CreateOfferResponse>();

            Assert.True(result.IsSuccess);

            return result.Id;
        }

        [Theory, AutoNSubstitute]
        public async Task GetOffers(GetOffersCommand getOfferCommand)
        {
            var result = await (await Route.WithClient(FlurlClient)
                                           .SendUrlEncodedAsync(HttpMethod.Get, getOfferCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public Task CreateOffer(CreateOfferCommand createOfferCommand,
                                CreateCourseCommand createCourseCommand,
                                CreateCampusCommand createCampusCommand,
                                CreateUniversityCommand createUniversityCommand) => 
            CreateOfferStatic(FlurlClient, createOfferCommand, createCourseCommand, createCampusCommand, createUniversityCommand);

        [Theory, AutoNSubstitute]
        public async Task UpdateOffer(UpdateOfferCommand updateOfferCommand,
                                       CreateOfferCommand createOfferCommand,
                                       CreateCourseCommand createCourseCommand,
                                       CreateCampusCommand createCampusCommand,
                                       CreateUniversityCommand createUniversityCommand)
        {
            updateOfferCommand.Id = await CreateOfferStatic(FlurlClient, createOfferCommand, createCourseCommand, createCampusCommand, createUniversityCommand);

            var result = await (await Route.WithClient(FlurlClient)
                                           .SendJsonAsync(HttpMethod.Put, updateOfferCommand))
                                           .GetJsonAsync<BaseResponse>();

            Assert.True(result.IsSuccess);
        }

        [Theory, AutoNSubstitute]
        public async Task DeleteOffer(DeleteOfferCommand deleteOfferCommand,
                                       CreateOfferCommand createOfferCommand,
                                       CreateCourseCommand createCourseCommand,
                                       CreateCampusCommand createCampusCommand,
                                       CreateUniversityCommand createUniversityCommand)
        {
            deleteOfferCommand.Id = await CreateOfferStatic(FlurlClient, createOfferCommand, createCourseCommand, createCampusCommand, createUniversityCommand);

            var result = await Route.WithClient(FlurlClient)
                                    .SendJsonAsync(HttpMethod.Delete, deleteOfferCommand);

            Assert.True(result.StatusCode == StatusCodes.Status204NoContent);
        }
    }
}
