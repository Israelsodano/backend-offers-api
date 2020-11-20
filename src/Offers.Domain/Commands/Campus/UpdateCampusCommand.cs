using System;
using Common.Result;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Campus
{
    public class UpdateCampusCommand : IRequest<Result<BaseResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public Guid UniversityId { get; set; }
    }
}
