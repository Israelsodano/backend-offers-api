using System.Collections.Generic;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Offer
{
    public class GetOffersResponse : BaseResponse
    {
        public IEnumerable<OfferResponse> Offers { get; set; }
    }
}
