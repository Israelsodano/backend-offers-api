using System;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.University
{
    public class DeleteUniversityCommand : IRequest<BaseResponse>
    {
        public Guid Id { get; set; }
    }
}
