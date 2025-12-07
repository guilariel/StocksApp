using MediatR;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.UseCases.DbUseCases;
using StocksApp.Application.Dtos.DbDtos;

namespace PurchaseStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<UsersDbDto>>> Get() =>
            Ok(await _mediator.Send(new GetAllUsersQuery()));
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UsersDbDto>> GetOne(int id) =>
            Ok(await _mediator.Send(new GetUserByIdQuery(id)));
        [HttpGet("name/{name}")]
        public async Task<ActionResult<UsersDbDto>> GetByName(string name) =>
            Ok(await _mediator.Send(new GetUserByNameQuery(name)));
    }
}
