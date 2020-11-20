using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Domain.Commands.Offer;

namespace Offers.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OfferController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<IActionResult> GetOffersPaged([FromQuery] GetOffersCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Created(string.Empty, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOffer([FromBody] UpdateOfferCommand command)
        {
            var result = await _mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOffer([FromQuery] DeleteOfferCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
