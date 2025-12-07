using Microsoft.AspNetCore.Mvc;
using PurchaseStocks.Application.UseCases;
using MediatR;
using SellStocks.Domain.Entities;
using SellStocks.Application.Dtos;
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
     