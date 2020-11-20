using System;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Course
{
    public class CreateCourseResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
