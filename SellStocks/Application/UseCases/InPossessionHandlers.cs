using MediatR;
using Microsoft.EntityFrameworkCore;
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

    public record DeleteInPossessionCommand(string user, string stock, int amount) : IRequest;
    public class DeleteInPossessionHandler : IRequestHandler<DeleteInPossessionCommand>
    {
        private readonly CrudPossession _service;
        private readonly CrudUsers _crudUsers;
        private readonly CrudStocks _crudStocks;
        private readonly CrudPrices _crudPrices;
        private readonly RabbitMessageService _messageService;

        public DeleteInPossessionHandler(CrudPossession service, CrudUsers crudUsers, CrudStocks crudStocks, CrudPrices crudPrices, RabbitMessageService messageService)
        {
            _service = service;
            _crudUsers = crudUsers;
            _crudStocks = crudStocks;
            _crudPrices = crudPrices;
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
                owner_id = user.id,
                stock_id = stock.id,
                amount = request.amount
            };
            await _messageService.SendMessage<InPossessionDb>(inPossessionDb,"sell");
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
            _crudStocks = crudStocks;
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
                StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, null);
                InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, pos.amount);
                result.Add(possessionDbDto);
            }
            return result;
        }
    }
}