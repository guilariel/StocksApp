using LogIn.Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellStocks.Application.Dtos;

namespace PurchaseStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            await _mediator.Send(new AddUserQuery(request.Name, request.Password));
            return Ok("Usuario agregado correctamente");
        }

        [Authorize]
        [HttpPost("AddFunds/{amount}/{currency}")]
        public async Task<IActionResult> AddFunds(double amount, string currency)
        {
            await _mediator.Send(new AddFundsQuery(amount, currency));
            return Ok("Fondos agregados correctamente");
        }
        [Authorize]
        [HttpDelete("SellFunds/{amount}/{currency}")]
        public async Task<IActionResult> SellFunds(double amount, string currency)
        {
            await _mediator.Send(new SellFundsQuery(amount, currency));
            return Ok("Fondos agregados correctamente");
        }
    }

    // Modelos de request
    public record LoginRequest(string Name, string Password);
    public record AddUserRequest(string Name, string Password);
}
