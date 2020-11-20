using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Result;
using MediatR;
using Offers.Domain.Commands.Campus;
using Offers.Domain.Entities;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Campus;
using Offers.Domain.Services;

namespace Offers.Application.Handlers
{
    public class CampusHandler : 
        IRequestHandler<GetCampusCommand, BaseResponse>,
        IRequestHandler<CreateCampusCommand, Result<BaseResponse>>,
        IRequestHandler<UpdateCampusCommand, Result<BaseResponse>>,
        IRequestHandler<DeleteCampusCommand, BaseResponse>
    {

        private readonly ICampusService _campusService;

        public CampusHandler(ICampusService campusService) => 
            _campusService = campusService ?? throw new ArgumentNullException(nameof(campusService));

        public async Task<Result<BaseResponse>> Handle(CreateCampusCommand request,
                                                       CancellationToken cancellationToken)
        {
            var result = await _campusService.CreateCampus(new Campus
            {
                Name = request.Name,
                City = request.City,
                UniversityId = request.UniversityId
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new CreateCampusResponse
                {
                    IsSuccess = false,
                    Message = result.Exception.Message
                });

            return Result.Ok<BaseResponse>(new CreateCampusResponse() { Id = result.Value.Id });
        }

        public async Task<BaseResponse> Handle(GetCampusCommand request,
                                                            CancellationToken cancellationToken)
        {
            var campus = await _campusService.GetPagedCampus(request.Range, request.Page,
                    new Campus { Name = request.Name, City = request.City, UniversityId = request.UniversityId },
                    request.OrderBy,
                    request.Desc);

            return new GetCampusResponse { Campus = campus };
        }

        public async Task<Result<BaseResponse>> Handle(UpdateCampusCommand request,
                                                             CancellationToken cancellationToken)
        {
            var result = await _campusService.UpdateCampus(request.Id, new Campus
            {
                Name = request.Name,
                City = request.City,
                UniversityId = request.UniversityId
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new UpdateCampusResponse
                {
                    Message = result.Exception.Message,
                    IsSuccess = false
                });

            return Result.Ok<BaseResponse>(new UpdateCampusResponse());
        }

        public async Task<BaseResponse> Handle(DeleteCampusCommand request,
                                               CancellationToken cancellationToken)
        {
            await _campusService.RemoveCampus(request.Id);
            return new DeleteCampusResponse();
        }
    }
}
