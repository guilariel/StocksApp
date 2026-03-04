using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksDll;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    public record GetAllPricesQuery() : IRequest<List<PriceDbDto>>;
    public class GetAllPricesHandler : IRequestHandler<GetAllPricesQuery, List<PriceDbDto>>
    {
        private readonly PriceRepository priceRepository;
        public GetAllPricesHandler(PriceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }
        public async Task<List<PriceDbDto>> Handle(GetAllPricesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<PriceHistoryDb> prices = await priceRepository.GetAllAsync();
            List<PriceDbDto> result = new List<PriceDbDto>();
            foreach (PriceHistoryDb pr in prices) 
            {
                PriceDbDto priceDbDto = new PriceDbDto(pr.price, pr.date);
                result.Add(priceDbDto);
            }
            return result;
        }
    }
    public record GetOnePriceQuery(string id) : IRequest<PriceDbDto>;

    public class GetOnePriceHandler : IRequestHandler<GetOnePriceQuery, PriceDbDto>
    {
        private readonly PriceRepository priceRepository;
        public GetOnePriceHandler(PriceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }
        public async Task<PriceDbDto> Handle(GetOnePriceQuery request, CancellationToken cancellationToken)
        {
            PriceHistoryDb? priceDb = await priceRepository.GetLatestAsync(request.id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            return priceDbDto;
        }
    }
}
/*
 *     public record AddOnePriceQuery(PriceDbDto price) : IRequest;
    public class AddOnePriceHandler : IRequestHandler<AddOnePriceQuery>
    {
        private readonly priceRepository priceRepository;
        public AddOnePriceHandler(priceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }
        public async Task Handle(AddOnePriceQuery request, CancellationToken cancellationToken)
        {
            await priceRepository.AddPrice(request.price);
        }
    }
    public record UpdateOnePriceQuery(PriceDbDto price) : IRequest;
    public class UpdateOnePriceHandler : IRequestHandler<UpdateOnePriceQuery>
    {
        private readonly priceRepository priceRepository;
        public UpdateOnePriceHandler(priceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }
        public async Task Handle(UpdateOnePriceQuery request, CancellationToken cancellationToken)
        {
            await priceRepository.UpdatePrice(request.price);
        }
    }
 * public record DeleteOnePriceQuery(int id) : IRequest;
    public class DeleteOnePriceHandler : IRequestHandler<DeleteOnePriceQuery> 
    {
        private readonly priceRepository priceRepository;
        public DeleteOnePriceHandler(priceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }
        public async Task Handle(DeleteOnePriceQuery request, CancellationToken cancellationToken)
        {
            await priceRepository.DeleteOnePrice(request.id);
        }
    }*/