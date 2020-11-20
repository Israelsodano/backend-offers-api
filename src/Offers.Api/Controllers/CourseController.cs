using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Domain.Commands.Course;

namespace Offers.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CourseController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<IActionResult> GetCoursesPaged([FromQuery] GetCoursesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Created(string.Empty, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromQuery] DeleteCourseCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
