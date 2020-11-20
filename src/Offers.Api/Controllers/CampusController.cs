using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Domain.Commands.Campus;

namespace Offers.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CampusController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CampusController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<IActionResult> GetCampusPaged([FromQuery] GetCampusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampus([FromBody] CreateCampusCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Created(string.Empty, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCampus([FromBody] UpdateCampusCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCampus([FromQuery] DeleteCampusCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
