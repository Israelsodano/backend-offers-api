using System.Collections.Generic;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Responses.Course
{
    public class GetCoursesResponse : BaseResponse
    {
        public IEnumerable<Entities.Course> Courses { get; set; }
    }
}
