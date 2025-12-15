using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksDll;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    // --- GET ALL ---
    public record GetAllStocksQuery() : IRequest<List<StockDbDto>>;

    public class GetAllStocksHandler : IRequestHandler<GetAllStocksQuery, List<StockDbDto>>
    {
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetAllStocksHandler(StockRepository StockRepository, PriceRepository priceService)
        {
            stockRepository = StockRepository;
            priceRepository = priceService;
        }

        public async Task<List<StockDbDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<StockDb> stocks = await stockRepository.GetAllAsync();
            List<StockDbDto> result = new List<StockDbDto>();
            foreach (StockDb st in stocks)
            {
                PriceDb? priceDb = await priceRepository.GetByIdAsync(st.price_id);
                PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
                StockDbDto stockDbDto = new StockDbDto(st.symbol, st.name, st.description, priceDbDto);
                result.Add(stockDbDto);
            }
            return result;
        }
    }

    // --- GET ONE ---
    public record GetStockByIdQuery(int Id) : IRequest<StockDbDto>;

    public class GetStockByIdHandler : IRequestHandler<GetStockByIdQuery, StockDbDto>
    {
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;

        public GetStockByIdHandler(StockRepository StockRepository, PriceRepository priceService)
        {
            stockRepository = StockRepository;
            priceRepository = priceService;
        }

        public async Task<StockDbDto> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
        {
            StockDb? stock =  await stockRepository.GetByIdAsync(request.Id);
            PriceDb priceDb = await priceRepository.GetByIdAsync(stock.price_id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            StockDbDto stockDbDto = new StockDbDto(stock.symbol,stock.name,stock.description,priceDbDto);
            return stockDbDto;
        }
    }
    public record GetStockByNameQuery(string name) : IRequest<StockDbDto>;
    public class GetStockByNameHandler : IRequestHandler<GetStockByNameQuery, StockDbDto>
    {
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetStockByNameHandler(StockRepository StockRepository, PriceRepository priceService)
        {
            stockRepository = StockRepository;
            priceRepository = priceService;
        }
        public async Task<StockDbDto> Handle(GetStockByNameQuery request, CancellationToken cancellationToken)
        {
            StockDb? stock = await stockRepository.GetOneByNameAsync(request.name);
            PriceDb priceDb = await priceRepository.GetByIdAsync(stock.price_id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            StockDbDto stockDbDto = new StockDbDto(stock.symbol, stock.name, stock.description, priceDbDto);
            return stockDbDto;
        }
    }
    public record GetStockPriceByNameQuery(string name) : IRequest<PriceDbDto?>;
    public class GetStockPriceByNameHandler : IRequestHandler<GetStockPriceByNameQuery, PriceDbDto?>
    {
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetStockPriceByNameHandler(StockRepository StockRepository, PriceRepository priceService)
        {
            stockRepository = StockRepository;
            priceRepository = priceService;
        }
        public async Task<PriceDbDto?> Handle(GetStockPriceByNameQuery request, CancellationToken cancellationToken)
        {
            StockDb? stockDb = await stockRepository.GetOneByNameAsync(request.name);
            PriceDb? priceDb = await priceRepository.GetByIdAsync(stockDb.price_id);
            if (priceDb == null)
            {
                return null;
            }
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            return priceDbDto;
        }
    }
}

/*
 *     // --- ADD ---
    public record AddStockCommand(StockDbDto Stock) : IRequest;

    public class AddStockHandler : IRequestHandler<AddStockCommand>
    {
        private readonly StockRepository stockRepository;

        public AddStockHandler(StockRepository StockRepository)
        {
            stockRepository = StockRepository;
        }

        public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            await stockRepository.AddAsync(request.Stock);
        }
    }

    // --- UPDATE ---
    public record UpdateStockCommand(StockDbDto Stock) : IRequest;

    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand>
    {
        private readonly StockRepository stockRepository;

        public UpdateStockHandler(StockRepository StockRepository)
        {
            stockRepository = StockRepository;
        }

        public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            await stockRepository.UpdateAsync(request.Stock);
        }
    }
// --- DELETE ---
    public record DeleteStockCommand(int Id) : IRequest;

    public class DeleteStockHandler : IRequestHandler<DeleteStockCommand>
    {
        private readonly StockRepository stockRepository;

        public DeleteStockHandler(StockRepository StockRepository)
        {
            stockRepository = StockRepository;
        }

        public async Task Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            await stockRepository.DeleteAsync(request.Id);
        }
    }*/