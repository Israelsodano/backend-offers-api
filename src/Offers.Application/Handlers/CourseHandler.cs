using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Result;
using MediatR;
using Offers.Domain.Commands.Course;
using Offers.Domain.Entities;
using Offers.Domain.Responses.Base;
using Offers.Domain.Responses.Course;
using Offers.Domain.Services;

namespace Offers.Application.Handlers
{
    public class CourseHandler :
        IRequestHandler<GetCoursesCommand, BaseResponse>,
        IRequestHandler<CreateCourseCommand, Result<BaseResponse>>,
        IRequestHandler<UpdateCourseCommand, Result<BaseResponse>>,
        IRequestHandler<DeleteCourseCommand, BaseResponse>
    {
        private readonly ICourseService _courseService;
        public CourseHandler(ICourseService courseService) => _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        public async Task<Result<BaseResponse>> Handle(CreateCourseCommand request,
                                                       CancellationToken cancellationToken)
        {
            var result = await _courseService.CreateCourse(new Course
            {
                Name = request.Name,
                Kind = request.Kind,
                Level = request.Level,
                Shift = request.Shift,
                CampusId = request.CampusId
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new CreateCourseResponse
                {
                    IsSuccess = false,
                    Message = result.Exception.Message
                });

            return Result.Ok<BaseResponse>(new CreateCourseResponse() { Id = result.Value.Id });
        }

        public async Task<BaseResponse> Handle(GetCoursesCommand request,
                                                            CancellationToken cancellationToken)
        {
            var courses = await _courseService.GetPagedCourses(request.Range, 
                                                               request.Page,
                                                               request.Name,
                                                               request.Kind,
                                                               request.Level,
                                                               request.Shift,
                                                               request.Desc);

            return new GetCoursesResponse { Courses = courses };
        }

        public async Task<Result<BaseResponse>> Handle(UpdateCourseCommand request,
                                                       CancellationToken cancellationToken)
        {
            var result = await _courseService.UpdateCourse(request.Id, new Course
            {
                Name = request.Name,
                Kind = request.Kind,
                Level = request.Level,
                Shift = request.Shift,
                CampusId = request.CampusId
            });

            if (!result.IsSuccess)
                return Result.Fail<BaseResponse>(result.Exception, new UpdateCourseResponse
                {
                    Message = result.Exception.Message,
                    IsSuccess = false
                });

            return Result.Ok<BaseResponse>(new UpdateCourseResponse());
        }

        public async Task<BaseResponse> Handle(DeleteCourseCommand request,
                                               CancellationToken cancellationToken)
        {
            await _courseService.RemoveCourse(request.Id);
            return new DeleteCourseResponse();
        }
    }
}
