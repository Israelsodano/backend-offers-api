using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Result;
using MediatR;
using Offers.Domain.Commands.Offer;
using Offers.Domain.Entities;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Offer;
using Offers.Domain.Services;

namespace Offers.Application.Handlers
{
    public class OfferHandler : 
        IRequestHandler<GetOffersCommand, BaseResponse>,
        IRequestHandler<CreateOfferCommand, Result<BaseResponse>>,
        IRequestHandler<UpdateOfferCommand, Result<BaseResponse>>,
        IRequestHandler<DeleteOfferCommand, BaseResponse>
    {

        private readonly IOfferService _OfferService;

        public OfferHandler(IOfferService OfferService) => 
            _OfferService = OfferService ?? throw new ArgumentNullException(nameof(OfferService));

        public async Task<Result<BaseResponse>> Handle(CreateOfferCommand request,
                                                       CancellationToken cancellationToken)
        {
            var result = await _OfferService.CreateOffer(new Offer
            {
                CourseId = request.CourseId,
                DiscountPercentage = request.DiscountPercentage,
                Enabled = true,
                FullPrice = request.FullPrice,
                StartDate = request.StartDate,
            }.SetEnrollmentSemester(request.Semester, request.EnrollmentSemesterYear)); 

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new CreateOfferResponse
                {
                    IsSuccess = false,
                    Message = result.Exception.Message
                });

            return Result.Ok<BaseResponse>(new CreateOfferResponse() { Id = result.Value.Id });
        }

        public async Task<BaseResponse> Handle(GetOffersCommand request,
                                               CancellationToken cancellationToken)
        {
            var Offer = await _OfferService.GetPagedOffers(request.Range, request.Page,
                    new Offer
                    {
                        CourseId = request.CourseId,
                        DiscountPercentage = request.DiscountPercentage,
                        Enabled = true,
                        FullPrice = request.FullPrice,
                        StartDate = request.StartDate,
                    }.SetEnrollmentSemester(request.Semester, request.EnrollmentSemesterYear),
                    request.OrderBy,
                    request.Desc);

            return new GetOffersResponse { Offers = Offer.Select(x=> (OfferResponse)x) };
        }

        public async Task<Result<BaseResponse>> Handle(UpdateOfferCommand request,
                                                       CancellationToken cancellationToken)
        {
            var result = await _OfferService.UpdateOffer(request.Id, new Offer
            {
                DiscountPercentage = request.DiscountPercentage,
                FullPrice = request.FullPrice,
                StartDate = request.StartDate,
            }, request.Semester, request.EnrollmentSemesterYear, request.Enabled);

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new UpdateOfferResponse
                {
                    Message = result.Exception.Message,
                    IsSuccess = false
                });

            return Result.Ok<BaseResponse>(new UpdateOfferResponse());
        }

        public async Task<BaseResponse> Handle(DeleteOfferCommand request,
                                               CancellationToken cancellationToken)
        {
            await _OfferService.RemoveOffer(request.Id);
            return new DeleteOfferResponse();
        }
    }
}
