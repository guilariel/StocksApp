using MediatR;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.UseCases.DbUseCases;
using StocksApp.Application.Dtos.DbDtos;

namespace PurchaseStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PricesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PriceDbDto>>> Get() =>
             Ok(await _mediator.Send(new GetAllPricesQuery()));
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceDbDto>> GetOne(int id) =>
             Ok(await _mediator.Send(new GetOnePriceQuery(id)));
    }
}
     