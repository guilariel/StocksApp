using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using StocksApp.Domain.Entities.DbEntities;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    // --- GET ALL ---
    public record GetAllInPossessionsQuery() : IRequest<List<InPossessionDbDto>>;

    public class GetAllInPossessionsHandler : IRequestHandler<GetAllInPossessionsQuery, List<InPossessionDbDto>>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;
        public GetAllInPossessionsHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks)
        {
            _service = service;
            _crudUsers = crudUsers;
            _crudStocks = crudStocks;
        }

        public async Task<List<InPossessionDbDto>> Handle(GetAllInPossessionsQuery request, CancellationToken cancellationToken)
        {
            List<InPossessionDb> possessions = await _service.GetAllAsync();
            List<InPossessionDbDto> result = new List<InPossessionDbDto>();
            foreach (InPossessionDb pos in possessions)
            {
                UsersDb user =  await _crudUsers.GetByIdAsync(pos.owner_id);
                StockDb stock = await _crudStocks.GetByIdAsync(pos.stock_id);
                UsersDbDto userDto = new UsersDbDto(user.name, user.funds);
                StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, null);
                InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto,stockDto,pos.amount);
                result.Add(possessionDbDto);
            }
            return result;
        }
    }

    // --- GET ONE ---
    public record GetInPossessionQuery(int OwnerId, int SymbolId) : IRequest<InPossessionDbDto?>;

    public class GetInPossessionHandler : IRequestHandler<GetInPossessionQuery, InPossessionDbDto?>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;

        public GetInPossessionHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks)
        {
            _service = service;
            _crudUsers = crudUsers;
            _crudStocks = crudStocks;
        }

        public async Task<InPossessionDbDto?> Handle(GetInPossessionQuery request, CancellationToken cancellationToken)
        {
            InPossessionDb possessionDb = await _service.GetOneAsync(request.OwnerId, request.SymbolId);
            UsersDb user = await _crudUsers.GetByIdAsync(possessionDb.owner_id);
            StockDb stock = await _crudStocks.GetByIdAsync(possessionDb.stock_id);
            UsersDbDto userDto = new UsersDbDto(user.name, user.funds);
            StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, null);
            InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, possessionDb.amount);
            return possessionDbDto;
        }
    }

    public record GetPossessionByNameQuery(string OwnerName) : IRequest<List<InPossessionDbDto>>;
    public class GetPossessionByNameHandler : IRequestHandler<GetPossessionByNameQuery, List<InPossessionDbDto>>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;
        private readonly CrudPrices _crudPrices;
        public GetPossessionByNameHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks, CrudPrices crudPrices)
        {
            _service = service;
            _crudUsers = crudUsers;
            _crudStocks = crudStocks;
            _crudPrices = crudPrices;
        }
        public async Task<List<InPossessionDbDto>> Handle(GetPossessionByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb? owner = await _crudUsers.GetOneByNameAsync(request.OwnerName);
            List<InPossessionDb> possessions = await _service.GetPossessionsByOwnerId(owner.id);
            List<InPossessionDbDto> result = new List<InPossessionDbDto>();
            foreach (InPossessionDb pos in possessions)
            {
                UsersDb user = await _crudUsers.GetByIdAsync(pos.owner_id);
                StockDb stock = await _crudStocks.GetByIdAsync(pos.stock_id);
                UsersDbDto userDto = new UsersDbDto(user.name, user.funds);
                PriceDb priceDb = await _crudPrices.GetOnePriceAsync(stock.price_id);
                PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
                StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, priceDbDto);
                InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, pos.amount);
                result.Add(possessionDbDto);
            }
            return result;
        }
    }
}