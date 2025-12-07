using MediatR;
using Microsoft.AspNetCore.Mvc;
using PurchaseStocks.Application.Handlers;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;
using StocksApp.Application.Dtos;
using System.Reflection.Metadata.Ecma335;

namespace PurchaseStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<StockDbDto>>> Get() =>
            Ok(await _mediator.Send(new GetAllStocksQuery()));
        [HttpGet("id/{id}")]
        public async Task<ActionResult<StockDbDto>> GetOne(int id) =>
            Ok(await _mediator.Send(new GetStockByIdQuery(id)));
        [HttpGet("name/{name}")]
        public async Task<ActionResult<StockDbDto>> GetOneByName(string name) =>
            Ok(await _mediator.Send(new GetStockByNameQuery(name)));
        [HttpGet("price/{name}")] 
        public async Task<ActionResult<PriceDbDto>> GetPrice(string name) =>
            Ok(await _mediator.Send(new GetStockPriceByNameQuery(name)));
    }
}
