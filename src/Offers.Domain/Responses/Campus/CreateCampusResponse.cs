using System;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Campus
{
    public class CreateCampusResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
