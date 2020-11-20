using System;
using Common.Result;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Campus
{
    public class CreateCampusCommand : IRequest<Result<BaseResponse>>
    {
        public Guid UniversityId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
