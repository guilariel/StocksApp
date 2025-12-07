using MediatR;
using StocksApp.Infrastructure.ExternalServices;

namespace StocksApp.Application.UseCases.DbUseCases
{
    public record class ActualizeDbByRabbitCommand() : IRequest;
    public class ActualizeDbByRabbitHandler : IRequestHandler<ActualizeDbByRabbitCommand>
    {
        private readonly UpdateDbs _updateDbs;
        private readonly CrudPossession crudPossession;
        private readonly CrudUsers crudUser;
        private readonly CrudStocks crudStock;
        private readonly CrudPrices crudPrice;
        public ActualizeDbByRabbitHandler(UpdateDbs updateDbs, CrudPossession crudPossession, CrudUsers crudUser, CrudStocks crudStock, CrudPrices crudPrice)
        {
            _updateDbs = updateDbs;
            this.crudPossession = crudPossession;
            this.crudUser = crudUser;
            this.crudStock = crudStock;
            this.crudPrice = crudPrice;
        }
        public async Task Handle(ActualizeDbByRabbitCommand request, CancellationToken cancellationToken)
        {
            var possessions = await crudPossession.GetAllAsync();
            var users = await crudUser.GetAllAsync();
            var stocks = await crudStock.GetAllAsync();
            var prices = await crudPrice.GetAllPricesAsync();
            await _updateDbs.UpdateAllDb(possessions, users, stocks, prices);
        }
    }
}
