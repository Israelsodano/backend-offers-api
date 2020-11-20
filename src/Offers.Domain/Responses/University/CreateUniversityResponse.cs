using System;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.University
{
    public class CreateUniversityResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
