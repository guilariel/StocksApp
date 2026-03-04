using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.RabbitMq;
using SellStocks.Application.Dtos;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using SellDll;
using StocksApp.Application.Dtos;
using PurchaseDll;
using PurchaseStocks.Application.Dtos;

namespace PurchaseStocks.Application.Handlers
{
    public record DeleteInPossessionCommand(string owner_name, string stock_name, double amount) : IRequest;
    public class DeleteInPossessionHandler : IRequestHandler<DeleteInPossessionCommand>
    {
        private readonly UserRepository _userRepository;
        private readonly StockRepository _stockRepository;
        private readonly RabbitMessageService _messageService;
        private readonly PriceRepository _priceRepository;
        public DeleteInPossessionHandler(UserRepository UserRepository, StockRepository StockRepository, RabbitMessageService messageService, PriceRepository priceRepository)
        {
            _userRepository = UserRepository;
            _stockRepository = StockRepository;
            _messageService = messageService;
            _priceRepository = priceRepository;
        }

        public async Task Handle(DeleteInPossessionCommand request, CancellationToken cancellationToken)
        {
            UsersDb? user = await _userRepository.GetOneByNameAsync(request.owner_name);
            StockDb? stock = await _stockRepository.GetOneByNameAsync(request.stock_name);
            PriceHistoryDb? price = await _priceRepository.GetByPriceIdAsync(stock.id);
            if (user == null || stock == null)
            {
                throw new Exception("User or Stock not found");
            }
            InPossessionDbDto possession = new InPossessionDbDto
            {
                owner_id = user.id,
                stock_id = stock.id,
                amount = request.amount
            };
            TransactionDto transaction = new TransactionDto
            {
                owner_id = user.id,
                stock_id = stock.id,
                amount = request.amount,
                price = price.price,
                currency = price.currency,
                date = DateTime.Now,
                type = "sell"
            };

            await _messageService.SendMessage<InPossessionDbDto>(possession, "sell");
            await _messageService.SendMessage<TransactionDto>(transaction, "add");
        }
    }
}