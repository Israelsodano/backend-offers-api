using System;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Campus
{
    public class DeleteCampusCommand : IRequest<BaseResponse>
    {
        public Guid Id { get; set; }
    }
}
