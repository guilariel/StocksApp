using MediatR;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.UseCases.DbUseCases;
using StocksApp.Application.Dtos.DbDtos;

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

        [HttpGet("ownerName/{ownerName}")]
        public async Task<ActionResult<List<InPossessionDbDto>>> GetPossessionByOwner(string ownerName) =>
            Ok(await _mediator.Send(new GetPossessionByNameQuery(ownerName)));
    }
}
