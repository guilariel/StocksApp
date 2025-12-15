using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.RabbitMq;
using SellStocks.Application.Dtos;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using SellDll;
using StocksApp.Application.Dtos;

namespace PurchaseStocks.Application.Handlers
{
    public record DeleteInPossessionCommand(string user, string stock, int amount) : IRequest;
    public class DeleteInPossessionHandler : IRequestHandler<DeleteInPossessionCommand>
    {
        private readonly UserRepository _crudUsers;
        private readonly StockRepository _crudStocks;
        private readonly RabbitMessageService _messageService;

        public DeleteInPossessionHandler(UserRepository UserRepository, StockRepository StockRepository, RabbitMessageService messageService)
        {
            _crudUsers = UserRepository;
            _crudStocks = StockRepository;
            _messageService = messageService;
        }

        public async Task Handle(DeleteInPossessionCommand request, CancellationToken cancellationToken)
        {
            StockDb stock = await _crudStocks.GetOneByNameAsync(request.stock);
            UsersDb user = await _crudUsers.GetOneByNameAsync(request.user);
            if(stock == null)
                throw new KeyNotFoundException("Stock not found");
            InPossessionDb inPossessionDb = new InPossessionDb
            {
                id = user.id,
                stock_id = stock.id,
                amount = request.amount
            };
            await _messageService.SendMessage<InPossessionDb>(inPossessionDb,"sell");
        }
    }
}