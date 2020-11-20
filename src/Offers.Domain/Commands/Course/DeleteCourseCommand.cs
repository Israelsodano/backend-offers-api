using System;
using MediatR;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Course
{
    public class DeleteCourseCommand : IRequest<BaseResponse>
    {
        public Guid Id { get; set; }
    }
}
