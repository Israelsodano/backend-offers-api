using System;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Campus
{
    public class GetCampusCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public string City { get; set; }
        public Guid UniversityId { get; set; }

        public int Page { get; set; }
        public int Range { get; set; }
        public string OrderBy { get; set; }
        public bool Desc { get; set; } = false;
    }
}
