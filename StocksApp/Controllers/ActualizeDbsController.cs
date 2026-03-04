using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitMQAndGenericRepository.RabbitMq;
using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Application.UseCases.DbUseCases;


namespace StocksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualizeDbsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ActualizeDbsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("ActualizeDbs")]
        public async Task<ActionResult> ActualizeDbs()
        {
            await _mediator.Send(new ActualizeDbByRabbitCommand());
            return Ok();
        }
    }
}
