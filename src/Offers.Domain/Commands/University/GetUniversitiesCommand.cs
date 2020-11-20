using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.University
{
    public class GetUniversitiesCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public float? Score { get; set; }

        public int Page { get; set; }
        public int Range { get; set; }
        public string OrderBy { get; set; }
        public bool Desc { get; set; } = false;
    }
}
