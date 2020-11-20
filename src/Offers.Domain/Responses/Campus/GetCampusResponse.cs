using System.Collections.Generic;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Campus
{
    public class GetCampusResponse : BaseResponse
    {
        public IEnumerable<Entities.Campus> Campus { get; set; }
    }
}
