using MediatR;
using Microsoft.AspNetCore.Mvc;
using PurchaseStocks.Application.Handlers;

namespace PurchaseStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InPossessionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InPossessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{user}/{stock}/{amount}")]
        public async Task<ActionResult> Delete(string user, string stock, int amount)
        {
            await _mediator.Send(new DeleteInPossessionCommand(user, stock, amount));
            return NoContent();
        }
    }
}

