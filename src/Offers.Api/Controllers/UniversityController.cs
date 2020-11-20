using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Domain.Commands.University;

namespace Offers.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UniversityController : ControllerBase
    {   
        private readonly IMediator _mediator;
        public UniversityController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<IActionResult> GetUniversitiesPaged([FromQuery] GetUniversitiesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Created(string.Empty, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUniversity([FromBody] UpdateUniversityCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUniversity([FromQuery] DeleteUniversityCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
