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
        [HttpGet("GetById/{owner_id}/{currency}")]
        public async Task<ActionResult<UsersDbDto>> GetOne(int owner_id, string currency) =>
            Ok(await _mediator.Send(new GetUserByIdQuery(owner_id, currency)));
        [HttpGet("GetByName/{owner_name}/{currency}")]
        public async Task<ActionResult<UsersDbDto>> GetByName(string owner_name, string currency) =>
            Ok(await _mediator.Send(new GetUserByNameQuery(owner_name, currency)));
        [HttpGet("GetYearlyProffit/{user}/{currency}")]
        public async Task<double> GetYearlyProffit(string user, string currency) =>
            await _mediator.Send(new GetYearlyProffitQuery(user, currency));
        [HttpGet("GetMonthlyProffit/{user}/{currency}")]
        public async Task<double> GetMonthlyProffit(string user, string currency) =>
            await _mediator.Send(new GetMonthlyProffitQuery(user, currency));
    }
}
