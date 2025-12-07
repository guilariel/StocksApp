using MediatR;
using PurchaseStocks.Infrastructure;
using SellStocks.Domain.Entities;
using SellStocks.Application.Dtos;
using StocksApp.Application.Dtos;

namespace PurchaseStocks.Application.Handlers
{
    // --- GET ALL ---
    public record GetAllStocksQuery() : IRequest<List<StockDbDto>>;

    public class GetAllStocksHandler : IRequestHandler<GetAllStocksQuery, List<StockDbDto>>
    {
        private readonly CrudStocks _stockService;
        private readonly CrudPrices _priceService;
        public GetAllStocksHandler(CrudStocks CrudStocks, CrudPrices priceService)
        {
            _stockService = CrudStocks;
            _priceService = priceService;
        }

        public async Task<List<StockDbDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            List<StockDb> stocks = await _stockService.GetAllAsync();
            List<StockDbDto> result = new List<StockDbDto>();
            foreach (StockDb st in stocks)
            {
                PriceDb priceDb = await _priceService.GetOnePrice(st.price_id);
                PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
                StockDbDto stockDbDto = new StockDbDto(st.symbol, st.name, st.description, priceDbDto);
                result.Add(stockDbDto);
            }
            return await Task.FromResult(result);
        }
    }

    // --- GET ONE ---
    public record GetStockByIdQuery(int Id) : IRequest<StockDbDto>;

    public class GetStockByIdHandler : IRequestHandler<GetStockByIdQuery, StockDbDto>
    {
        private readonly CrudStocks _stockService;
        private readonly CrudPrices _priceService;

        public GetStockByIdHandler(CrudStocks CrudStocks, CrudPrices priceService)
        {
            _stockService = CrudStocks;
            _priceService = priceService;
        }

        public async Task<StockDbDto> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
        {
            StockDb? stock =  await _stockService.GetByIdAsync(request.Id);
            PriceDb priceDb = await _priceService.GetOnePrice(stock.price_id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            StockDbDto stockDbDto = new StockDbDto(stock.symbol,stock.name,stock.description,priceDbDto);
            return await Task.FromResult(stockDbDto);
        }
    }
    public record GetStockByNameQuery(string name) : IRequest<StockDbDto>;
    public class GetStockByNameHandler : IRequestHandler<GetStockByNameQuery, StockDbDto>
    {
        private readonly CrudStocks _stockService;
        private readonly CrudPrices _priceService;
        public GetStockByNameHandler(CrudStocks CrudStocks, CrudPrices priceService)
        {
            _stockService = CrudStocks;
            _priceService = priceService;
        }
        public async Task<StockDbDto> Handle(GetStockByNameQuery request, CancellationToken cancellationToken)
        {
            StockDb? stock = await _stockService.GetOneByNameAsync(request.name);
            PriceDb priceDb = await _priceService.GetOnePrice(stock.price_id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            StockDbDto stockDbDto = new StockDbDto(stock.symbol, stock.name, stock.description, priceDbDto);
            return await Task.FromResult(stockDbDto);
        }
    }
    public record GetStockPriceByNameQuery(string name) : IRequest<PriceDbDto?>;
    public class GetStockPriceByNameHandler : IRequestHandler<GetStockPriceByNameQuery, PriceDbDto?>
    {
        private readonly CrudStocks _stockService;
        public GetStockPriceByNameHandler(CrudStocks CrudStocks, CrudPrices priceService)
        {
            _stockService = CrudStocks;
        }
        public async Task<PriceDbDto?> Handle(GetStockPriceByNameQuery request, CancellationToken cancellationToken)
        {
            PriceDb? priceDb = await _stockService.GetStockPriceByNameAsync(request.name);
            if (priceDb == null)
            {
                return null;
            }
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            return await Task.FromResult(priceDbDto);
        }
    }
}

/*
 *     // --- ADD ---
    public record AddStockCommand(StockDbDto Stock) : IRequest;

    public class AddStockHandler : IRequestHandler<AddStockCommand>
    {
        private readonly CrudStocks _stockService;

        public AddStockHandler(CrudStocks CrudStocks)
        {
            _stockService = CrudStocks;
        }

        public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            await _stockService.AddAsync(request.Stock);
        }
    }

    // --- UPDATE ---
    public record UpdateStockCommand(StockDbDto Stock) : IRequest;

    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand>
    {
        private readonly CrudStocks _stockService;

        public UpdateStockHandler(CrudStocks CrudStocks)
        {
            _stockService = CrudStocks;
        }

        public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            await _stockService.UpdateAsync(request.Stock);
        }
    }
// --- DELETE ---
    public record DeleteStockCommand(int Id) : IRequest;

    public class DeleteStockHandler : IRequestHandler<DeleteStockCommand>
    {
        private readonly CrudStocks _stockService;

        public DeleteStockHandler(CrudStocks CrudStocks)
        {
            _stockService = CrudStocks;
        }

        public async Task Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            await _stockService.DeleteAsync(request.Id);
        }
    }*/