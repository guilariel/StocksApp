using MediatR;
using PurchaseStocks.Infrastructure;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace PurchaseStocks.Application.UseCases
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
            List<PriceDb> prices = await crudPrices.GetAllPrices();
            List<PriceDbDto> result = new List<PriceDbDto>();
            foreach (PriceDb pr in prices) 
            {
                PriceDbDto priceDbDto = new PriceDbDto(pr.price, pr.date);
                result.Add(priceDbDto);
            }
            return await Task.FromResult(result);
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
            PriceDb priceDb = await crudPrices.GetOnePrice(request.id);
            PriceDbDto priceDbDto = new PriceDbDto(priceDb.price, priceDb.date);
            return await Task.FromResult(priceDbDto);
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