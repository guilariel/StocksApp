using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using StocksApp.Domain.Entities.DbEntities;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    public record GetAllPricesQuery() : IRequest<List<PriceDbDto>>;
    public class GetAllPricesHandler : IRequestHandler<GetAllPricesQuery, List<PriceDbDto>>
    {
        private readonly CrudPrices crudPrices;
        public GetAllPricesHandler(CrudPrices crudPrices)
        {
            this.crudPrices = crudPrices;
        }
        public async Task<List<PriceDbDto>> Handle(GetAllPricesQuery request, CancellationToken cancellationToken)
        {
            List<PriceDb> prices = await crudPrices.GetAllPricesAsync();
            List<PriceDbDto> result = new List<PriceDbDto>();
            foreach (PriceDb pr in prices) 
            {
                PriceDbDto priceDbDto = new PriceDbDto(pr.price, pr.date);
                result.Add(priceDbDto);
            }
            return result;
        }
    }
    public record GetOnePriceQuery(int id) : IRequest<PriceDbDto>;

    public class GetOnePriceHandler : IRequestHandler<GetOnePriceQuery, PriceDbDto>
    {
        private readonly CrudPrices crudPrices;
        public GetOnePriceHandler(CrudPrices crudPrices)
        {
            this.crudPrices = crudPrices;
        }
        public async Task<PriceDbDto> Handle(GetOnePriceQuery request, CancellationToken cancellationToken)
        {
            PriceDb priceDb = await crudPrices.GetOnePriceAsync(request.id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            return priceDbDto;
        }
    }
}
/*
 *     public record AddOnePriceQuery(PriceDbDto price) : IRequest;
    public class AddOnePriceHandler : IRequestHandler<AddOnePriceQuery>
    {
        private readonly CrudPrices crudPrices;
        public AddOnePriceHandler(CrudPrices crudPrices)
        {
            this.crudPrices = crudPrices;
        }
        public async Task Handle(AddOnePriceQuery request, CancellationToken cancellationToken)
        {
            await crudPrices.AddPrice(request.price);
        }
    }
    public record UpdateOnePriceQuery(PriceDbDto price) : IRequest;
    public class UpdateOnePriceHandler : IRequestHandler<UpdateOnePriceQuery>
    {
        private readonly CrudPrices crudPrices;
        public UpdateOnePriceHandler(CrudPrices crudPrices)
        {
            this.crudPrices = crudPrices;
        }
        public async Task Handle(UpdateOnePriceQuery request, CancellationToken cancellationToken)
        {
            await crudPrices.UpdatePrice(request.price);
        }
    }
 * public record DeleteOnePriceQuery(int id) : IRequest;
    public class DeleteOnePriceHandler : IRequestHandler<DeleteOnePriceQuery> 
    {
        private readonly CrudPrices crudPrices;
        public DeleteOnePriceHandler(CrudPrices crudPrices)
        {
            this.crudPrices = crudPrices;
        }
        public async Task Handle(DeleteOnePriceQuery request, CancellationToken cancellationToken)
        {
            await crudPrices.DeleteOnePrice(request.id);
        }
    }*/