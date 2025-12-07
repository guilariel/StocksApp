using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.RabbitMq;
using StocksApp.Domain.Entities.DbEntities;
namespace StocksApp.Infrastructure.ExternalServices
{
    public class UpdateDbs
    {
        private readonly AppDbContext _context;
        private RabbitMessageService _rabbitMessageService;
        public UpdateDbs(AppDbContext context, RabbitMessageService rabbitMessageService)
        {
            _context = context;
            _rabbitMessageService = rabbitMessageService;
        }
        public async Task UpdateAllDb(List<InPossessionDb> possessions, List<UsersDb> users, List<StockDb> stocks, List<PriceDb> prices)
        {
            foreach(var possession in possessions)
                await _rabbitMessageService.SendMessage<InPossessionDb>(possession, "add");
            foreach(var user in users)
                await _rabbitMessageService.SendMessage<UsersDb>(user, "add");
            foreach(var stock in stocks)
                await _rabbitMessageService.SendMessage<StockDb>(stock, "add");
            foreach(var price in prices)
                await _rabbitMessageService.SendMessage<PriceDb>(price, "add");
        }
    }
}
