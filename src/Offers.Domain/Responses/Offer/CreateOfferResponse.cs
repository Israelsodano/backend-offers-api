using System;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Offer
{
    public class CreateOfferResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
