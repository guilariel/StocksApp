using MediatR;
using Microsoft.AspNetCore.Mvc;
using PurchaseStocks.Application.Handlers;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;

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
        [HttpGet]
        public async Task<ActionResult<List<InPossessionDbDto>>> Get() =>
            Ok(await _mediator.Send(new GetAllInPossessionsQuery()));
        [HttpGet("{id}/{user}")]
        public async Task<ActionResult<InPossessionDbDto>> GetOne(int id, int user) =>
            Ok(await _mediator.Send(new GetInPossessionQuery(id,user)));
        [HttpDelete("{user}/{stock}/{amount}")]
        public async Task<ActionResult> Delete(string user, string stock, int amount)
        {
            await _mediator.Send(new DeleteInPossessionCommand(user,stock,amount));
            return NoContent();
        }
        [HttpGet("ownerName/{ownerName}")]
        public async Task<ActionResult<List<InPossessionDbDto>>> GetPossessionByOwner(string ownerName) =>
            Ok(await _mediator.Send(new GetPossessionByNameQuery(ownerName)));
    }
}
