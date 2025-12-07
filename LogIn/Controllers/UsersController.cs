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

        [HttpGet]
        public async Task<ActionResult<List<UsersDbDto>>> Get() =>
            Ok(await _mediator.Send(new GetAllUsersQuery()));

        [HttpGet("id/{id}")]
        public async Task<ActionResult<UsersDbDto>> GetOne(int id) =>
            Ok(await _mediator.Send(new GetUserByIdQuery(id)));

        [HttpGet("name/{name}")]
        public async Task<ActionResult<UsersDbDto>> GetByName(string name) =>
            Ok(await _mediator.Send(new GetUserByNameQuery(name)));

        [HttpPost("login")]
        public async Task<IActionResult> UserLogIn([FromBody] LoginRequest request)
        {
            var token = await _mediator.Send(new UserLogInQuery(request.Name, request.Password));

            if (token == null)
                return Unauthorized("Nombre o contraseña incorrectos");

            return Ok(new { token });
        }//como arrancar una api de .net por powershell

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            await _mediator.Send(new AddUserQuery(request.Name, request.Password));
            return Ok("Usuario agregado correctamente");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userName = User.Identity?.Name ?? "Usuario";
            return Ok(new { message = $"Hola {userName}, estás autenticado ✅" });
        }
        [HttpPost("AddFunds/{name}/{amount}")]
        public async Task<IActionResult> AddFunds(string name, double amount)
        {
            await _mediator.Send(new AddFundsQuery(name, amount));
            return Ok("Fondos agregados correctamente");
        }
    }

    // Modelos de request
    public record LoginRequest(string Name, string Password);
    public record AddUserRequest(string Name, string Password);
}
