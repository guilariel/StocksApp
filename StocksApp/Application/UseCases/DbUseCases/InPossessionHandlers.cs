using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksDll;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    // --- GET ALL ---
    public record GetAllInPossessionsQuery() : IRequest<List<InPossessionDbDto>>;

    public class GetAllInPossessionsHandler : IRequestHandler<GetAllInPossessionsQuery, List<InPossessionDbDto>>
    {
        private readonly InPossessionRepository possessionRepository;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;
        public GetAllInPossessionsHandler(InPossessionRepository service, UserRepository UserRepository, StockRepository StockRepository)
        {
            possessionRepository = service;
            userRepository = UserRepository;
            stockRepository = StockRepository;
        }

        public async Task<List<InPossessionDbDto>> Handle(GetAllInPossessionsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<InPossessionDb> possessions = await possessionRepository.GetAllAsync();
            List<InPossessionDbDto> result = new List<InPossessionDbDto>();
            foreach (InPossessionDb pos in possessions)
            {
                UsersDb? user =  await userRepository.GetByIdAsync(pos.owner_id);
                StockDb? stock = await stockRepository.GetByIdAsync(pos.stock_id);
                UsersDbDto userDto = new UsersDbDto(user.name);
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
        private readonly InPossessionRepository possessionRepository;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;

        public GetInPossessionHandler(InPossessionRepository service, UserRepository UserRepository, StockRepository StockRepository)
        {
            possessionRepository = service;
            userRepository = UserRepository;
            stockRepository = StockRepository;
        }

        public async Task<InPossessionDbDto?> Handle(GetInPossessionQuery request, CancellationToken cancellationToken)
        {
            InPossessionDb? possessionDb = await possessionRepository.GetByIdAsync(request.OwnerId, request.SymbolId);
            UsersDb? user = await userRepository.GetByIdAsync(possessionDb.owner_id);
            StockDb? stock = await stockRepository.GetByIdAsync(possessionDb.stock_id);
            UsersDbDto userDto = new UsersDbDto(user.name);
            StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, null);
            InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, possessionDb.amount);
            return possessionDbDto;
        }
    }

    public record GetPossessionByNameQuery(string OwnerName) : IRequest<List<InPossessionDbDto>>;
    public class GetPossessionByNameHandler : IRequestHandler<GetPossessionByNameQuery, List<InPossessionDbDto>>
    {
        private readonly InPossessionRepository possessionRepository;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetPossessionByNameHandler(InPossessionRepository service, UserRepository UserRepository, StockRepository StockRepository, PriceRepository PriceRepository)
        {
            possessionRepository = service;
            userRepository = UserRepository;
            stockRepository = StockRepository;
            priceRepository = PriceRepository;
        }
        public async Task<List<InPossessionDbDto>> Handle(GetPossessionByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb? owner = await userRepository.GetOneByNameAsync(request.OwnerName);
            List<InPossessionDb> possessions = await possessionRepository.GetAllByOwnerAsync(owner.id);
            List<InPossessionDbDto> result = new List<InPossessionDbDto>();
            foreach (InPossessionDb pos in possessions)
            {
                UsersDb? user = await userRepository.GetByIdAsync(pos.owner_id);
                StockDb? stock = await stockRepository.GetByIdAsync(pos.stock_id);
                UsersDbDto? userDto = new UsersDbDto(user.name);
                PriceHistoryDb? priceDb = await priceRepository.GetLatestAsync(stock.id);
                PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
                StockDbDto stockDto = new StockDbDto(stock.symbol, stock.name, stock.description, priceDbDto);
                InPossessionDbDto possessionDbDto = new InPossessionDbDto(userDto, stockDto, pos.amount);
                result.Add(possessionDbDto);
            }
            return result;
        }
    }
}