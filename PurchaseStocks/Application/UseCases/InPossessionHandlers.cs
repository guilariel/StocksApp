using MediatR;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using PurchaseDll;

namespace PurchaseStocks.Application.Handlers
{
    public record AddPossessionCommand(string owner_name, string stock_name, int amount) : IRequest;
    public class AddPossessionHandler : IRequestHandler<AddPossessionCommand>
    {
        private readonly UserRepository _crudUsers;
        private readonly StockRepository _crudStocks;
        private readonly RabbitMessageService _messageService;

        public AddPossessionHandler(UserRepository UserRepository, StockRepository StockRepository, RabbitMessageService messageService)
        {
            _crudUsers = UserRepository;
            _crudStocks = StockRepository;
            _messageService = messageService;
        }

        public async Task Handle(AddPossessionCommand request, CancellationToken cancellationToken)
        {
            UsersDb? user = await _crudUsers.GetOneByNameAsync(request.owner_name);
            StockDb? stock = await _crudStocks.GetOneByNameAsync(request.stock_name);
            if (user == null || stock == null)
            {
                throw new Exception("User or Stock not found");
            }
            InPossessionDb possession = new InPossessionDb
            {
                id = user.id,
                stock_id = stock.id,
                amount = request.amount
            };
            await _messageService.SendMessage<InPossessionDb>(possession, "add");
        }
    }
}

/*  // --- DELETE ---
    public record DeleteInPossessionCommand(int OwnerId, string SymbolId) : IRequest;

    public class DeleteInPossessionHandler : IRequestHandler<DeleteInPossessionCommand>
    {
        private readonly CrudPossession _service;

        public DeleteInPossessionHandler(CrudPossession service)
        {
            _service = service;
        }

        public async Task Handle(DeleteInPossessionCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(request.OwnerId, request.SymbolId);
        }
    }*/