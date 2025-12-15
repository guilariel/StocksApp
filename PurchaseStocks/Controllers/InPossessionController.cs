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
   
        [HttpPost("{owner_name}/{stock_name}/{amount}")]
        public async Task<ActionResult> Add(string owner_name, string stock_name, int amount)
        {
            await _mediator.Send(new AddPossessionCommand(owner_name,stock_name,amount));
            return Ok();
        }
    }
}
