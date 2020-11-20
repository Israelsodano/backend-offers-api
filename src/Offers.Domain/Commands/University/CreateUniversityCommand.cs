using Common.Result;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.University
{
    public class CreateUniversityCommand : IRequest<Result<BaseResponse>>
    {
        public string Name { get; set; }
        public float Score { get; set; }
        public string LogoUrl { get; set; }
    }
}
