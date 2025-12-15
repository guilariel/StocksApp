using MediatR;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksApp.Infrastructure.ExternalServices;
using StocksDll;

namespace StocksApp.Application.UseCases.DbUseCases
{
    public record class ActualizeDbByRabbitCommand() : IRequest;
    public class ActualizeDbByRabbitHandler : IRequestHandler<ActualizeDbByRabbitCommand>
    {
        private readonly UpdateDbs _updateDbs;
        private readonly InPossessionRepository possessionRepository;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public ActualizeDbByRabbitHandler(UpdateDbs updateDbs, InPossessionRepository service, UserRepository UserRepository, StockRepository StockRepository, PriceRepository PriceRepository)
        {
            _updateDbs = updateDbs;
            possessionRepository = service;
            userRepository = UserRepository;
            stockRepository = StockRepository;
            priceRepository = PriceRepository;
        }
        public async Task Handle(ActualizeDbByRabbitCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<InPossessionDb> possessions = await possessionRepository.GetAllAsync();
            IEnumerable<UsersDb> users = await userRepository.GetAllAsync();
            IEnumerable<StockDb> stocks = await stockRepository.GetAllAsync();
            IEnumerable<PriceDb> prices = await priceRepository.GetAllAsync();
            await _updateDbs.UpdateAllDb(possessions, users, stocks, prices);
        }
    }
}
