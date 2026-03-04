using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksDll;
using StocksApp.Application.Dtos.DbDtos;
using ActualizeDataBaseWithRabbitMQ.Repositories;

namespace StocksApp.Application.UseCases.DbUseCases
{
    // --- GET ALL ---
    public record GetAllTransactionsQuery() : IRequest<List<TransactionHistoryDto>>;

    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionHistoryDto>>
    {
        private readonly TransactionRepository transactionHistoryDb;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetAllTransactionsHandler(TransactionRepository TransactionRepository, UserRepository UserRepository, StockRepository StockRepository, PriceRepository PriceRepository)
        {
            transactionHistoryDb = TransactionRepository;
            userRepository = UserRepository;
            stockRepository = StockRepository;
            priceRepository = PriceRepository;
        }

        public async Task<List<TransactionHistoryDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHistoryDb> transactions = await transactionHistoryDb.GetAllAsync();
            List<TransactionHistoryDto> result = new List<TransactionHistoryDto>();
            foreach (TransactionHistoryDb i in transactions)
            {
                UsersDb? user = await userRepository.GetByIdAsync(i.owner_id);
                StockDb? stock = await stockRepository.GetByIdAsync(i.stock_id);
                PriceHistoryDb? price = await priceRepository.GetLatestAsync(stock.id);
                TransactionHistoryDto transactionDto = new TransactionHistoryDto(
                    i.owner_id,
                    i.stock_id,
                    i.amount,
                    price.price,
                    price.currency
                );
                result.Add(transactionDto);
            }
            return result;
        }
    }

    //get all by owner id 

    public record GetTransactionsByOwnerQuery(int owner_id) : IRequest<List<TransactionHistoryDto>>;

   public class GetTransactionsByOwnerHandler : IRequestHandler<GetTransactionsByOwnerQuery, List<TransactionHistoryDto>>
    {
        private readonly TransactionRepository transactionHistoryDb;
        private readonly UserRepository userRepository;
        private readonly StockRepository stockRepository;
        private readonly PriceRepository priceRepository;
        public GetTransactionsByOwnerHandler(TransactionRepository TransactionRepository, UserRepository UserRepository, StockRepository StockRepository, PriceRepository PriceRepository)
        {
            transactionHistoryDb = TransactionRepository;
            userRepository = UserRepository;
            stockRepository = StockRepository;
            priceRepository = PriceRepository;
        }
        public async Task<List<TransactionHistoryDto>> Handle(GetTransactionsByOwnerQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHistoryDb> transactions = await transactionHistoryDb.GetByOwnerIdAsync(request.owner_id);
            List<TransactionHistoryDto> result = new List<TransactionHistoryDto>();
            foreach (TransactionHistoryDb i in transactions)
            {
                UsersDb? user = await userRepository.GetByIdAsync(i.owner_id);
                StockDb? stock = await stockRepository.GetByIdAsync(i.stock_id);
                PriceHistoryDb? price = await priceRepository.GetLatestAsync(stock.id);
                TransactionHistoryDto transactionDto = new TransactionHistoryDto(
                    i.owner_id,
                    i.stock_id,
                    i.amount,
                    price.price,
                    price.currency
                );
                result.Add(transactionDto);
            }
            return result;
        }
    }
}