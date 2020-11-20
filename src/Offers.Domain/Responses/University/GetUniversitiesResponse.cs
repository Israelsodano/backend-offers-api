using System.Collections.Generic;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.University
{
    public class GetUniversitiesResponse : BaseResponse
    {
        public IEnumerable<Entities.University> Universities { get; set; }
    }
}
