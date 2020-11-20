using System;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Offer
{
    public class DeleteOfferCommand : IRequest<BaseResponse>
    {
        public Guid Id { get; set; }
    }
}
