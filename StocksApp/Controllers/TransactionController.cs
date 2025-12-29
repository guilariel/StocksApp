using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksApp.Application.UseCases.DbUseCases;
using static StocksApp.Application.UseCases.DbUseCases.GetTransactionsByOwnerHandler;
namespace StocksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<TransactionHistoryDb>> Get() =>
            Ok(await _mediator.Send(new GetAllTransactionsQuery()));
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionHistoryDb>> GetByOwner(int id) =>
            Ok(await _mediator.Send(new GetTransactionsByOwnerQuery(id)));
    }
}
