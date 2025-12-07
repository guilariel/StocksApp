using MediatR;
using PurchaseStocks.Infrastructure;
using RabbitMQAndGenericRepository.RabbitMq;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;
using StocksApp.Application.Dtos;

namespace PurchaseStocks.Application.Handlers
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
            return await Task.FromResult(result);
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
            return await Task.FromResult(possessionDbDto);
        }
    }

    public record AddPossessionCommand(string owner_name, string stock_name, int amount) : IRequest;
    public class AddPossessionHandler : IRequestHandler<AddPossessionCommand>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;
        private readonly RabbitMessageService _rabbitMessageService;
        public AddPossessionHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks, RabbitMessageService rabbitMessageService)
        {
            _service = service;
            _crudUsers = crudUsers;
            _crudStocks = crudStocks;
            _rabbitMessageService = rabbitMessageService;
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
                owner_id = user.id,
                stock_id = stock.id,
                amount = request.amount
            };
            await _rabbitMessageService.SendMessage<InPossessionDb>(possession, "add");
        }
    }
    public record GetPossessionByNameQuery(string OwnerName) : IRequest<List<InPossessionDbDto>>;
    public class GetPossessionByNameHandler : IRequestHandler<GetPossessionByNameQuery, List<InPossessionDbDto>>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;
        public GetPossessionByNameHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks)
        {
            _service = service;
            _crudUsers = crudUsers;
        }
        public async Task<List<InPossessionDbDto>> Handle(GetPossessionByNameQuery request, CancellationToken cancellationToken)
        {
            List<InPossessionDb> possessions = await _service.GetPossessionsByOwner(request.OwnerName,_crudUsers);
            List<InPossessionDbDto> result = new List<InPossessionDbDto>();
            foreach (InPossessionDb pos in possessions)
            {
                UsersDb user = await _crudUsers.GetByIdAsync(pos.owner_id);
                StockDb stock = await _crudStocks.GetByIdAsync(pos.stock_id);
                UsersDbDto userDto = new UsersDbDto(user.name, user.funds);
                StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, null);
                InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, pos.amount);
                result.Add(possessionDbDto);
            }
            return await Task.FromResult(result);
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