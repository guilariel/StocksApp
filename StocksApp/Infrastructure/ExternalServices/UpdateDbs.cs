using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using Microsoft.EntityFrameworkCore;
namespace StocksApp.Infrastructure.ExternalServices
{
    public class UpdateDbs
    {
        private readonly DbContext _context;
        private RabbitMessageService _rabbitMessageService;
        public UpdateDbs(DbContext context, RabbitMessageService rabbitMessageService)
        {
            _context = context;
            _rabbitMessageService = rabbitMessageService;
        }
        public async Task UpdateAllDb(IEnumerable<InPossessionDb> possessions, IEnumerable<UsersDb> users, IEnumerable<StockDb> stocks, IEnumerable<PriceHistoryDb> prices)
        {
            foreach(var possession in possessions)
                await _rabbitMessageService.SendMessage<InPossessionDb>(possession, "add");
            foreach(var user in users)
                await _rabbitMessageService.SendMessage<UsersDb>(user, "add");
            foreach(var stock in stocks)
                await _rabbitMessageService.SendMessage<StockDb>(stock, "add");
            foreach(var price in prices)
                await _rabbitMessageService.SendMessage<PriceHistoryDb>(price, "add");
        }
    }
}

