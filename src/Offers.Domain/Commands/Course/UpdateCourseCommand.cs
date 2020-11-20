using System;
using Common.Result;
using MediatR;
using Offers.Domain.Entities.Enums;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Course
{
    public class UpdateCourseCommand : IRequest<Result<BaseResponse>>
    {
        public Guid Id { get; set; }
        public Guid CampusId { get; set; }
        public string Name { get; set; }
        public Kind Kind { get; set; }
        public Level Level { get; set; }
        public Shift Shift { get; set; }
    }
}
