using System.Threading;
using System.Threading.Tasks;
using Common.Result;
using MediatR;
using Offers.Domain.Commands.University;
using Offers.Domain.Entities;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.University;
using Offers.Domain.Services;

namespace Offers.Application.Handlers
{
    public class UniversityHandler : 
        IRequestHandler<CreateUniversityCommand, Result<BaseResponse>>,
        IRequestHandler<GetUniversitiesCommand, BaseResponse>,
        IRequestHandler<UpdateUniversityCommand, Result<BaseResponse>>,
        IRequestHandler<DeleteUniversityCommand, BaseResponse>
    {
        private readonly IUniversityService _universityService;
        public UniversityHandler(IUniversityService universityService) => _universityService = universityService;

        public async Task<Result<BaseResponse>> Handle(CreateUniversityCommand request, 
                                                       CancellationToken cancellationToken)
        {
            var result = await _universityService.CreateUniversity(new University 
            { 
                Name = request.Name,
                LogoUrl = request.LogoUrl,
                Score = request.Score
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new CreateUniversityResponse 
                { 
                    IsSuccess = false,
                    Message = result.Exception.Message
                });

            return Result.Ok<BaseResponse>(new CreateUniversityResponse() { Id = result.Value.Id });
        }

        public async Task<BaseResponse> Handle(GetUniversitiesCommand request, 
                                                            CancellationToken cancellationToken)
        {
            var universities = await _universityService.GetPagedUniversities(request.Range, request.Page,
                    new University { Name = request.Name, Score = request.Score ?? 0 },
                    request.OrderBy,
                    request.Desc);

            return new GetUniversitiesResponse { Universities = universities };
        }

        public async Task<Result<BaseResponse>> Handle(UpdateUniversityCommand request, 
                                                             CancellationToken cancellationToken)
        {
            var result = await _universityService.UpdateUniversity(request.Id, new University 
            { 
                Name = request.Name,
                Score = request.Score ?? 0,
                LogoUrl = request.LogoUrl
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new UpdateUniversityResponse 
                { 
                    Message = result.Exception.Message,
                    IsSuccess = false
                });

            return Result.Ok<BaseResponse>(new UpdateUniversityResponse());
        }

        public async Task<BaseResponse> Handle(DeleteUniversityCommand request, 
                                               CancellationToken cancellationToken)
        {
            await _universityService.RemoveUniversity(request.Id);
            return new DeleteUniversityResponse();
        }
    }
}
