using System;
using MediatR;
using Offers.Domain.Entities.Enums;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Course
{
    public class GetCoursesCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public Kind? Kind { get; set; }
        public Level? Level { get; set; }
        public Shift? Shift { get; set; }
        public Guid CampusId { get; set; }

        public int Page { get; set; }
        public int Range { get; set; }
        public bool Desc { get; set; } = false;
    }
}
