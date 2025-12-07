using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StocksApp.Domain.Entities.DbEntities;

namespace StocksApp.Infrastructure.ExternalServices
{
    public class CrudPrices
    {
        private readonly AppDbContext appDbContext;
        public CrudPrices(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddPriceAsync(DateTime date, double price)
        {
            PriceDb priceDb = new PriceDb { date = date, price = price }; 
            appDbContext.Add(priceDb);
            await appDbContext.SaveChangesAsync();
        }
        public async Task UpdatePriceAsync(int id, DateTime date, double price)
        {
            PriceDb existing = await appDbContext.price.FirstOrDefaultAsync(p => p.id == id);
            if (existing == null)
                throw new KeyNotFoundException("Price not found");;
            existing.date = date;
            existing.price = price; 
            await appDbContext.SaveChangesAsync();

        }
        public async Task<List<PriceDb>> GetAllPricesAsync()
        {
             var prices = await appDbContext.price.ToListAsync();           
             return prices;   
        }
        public async Task<PriceDb> GetOnePriceAsync(int id)
        {
            var price = await appDbContext.price.AsNoTracking().SingleOrDefaultAsync(u => u.id == id);
            if (price == null)
                throw new KeyNotFoundException("there´s no id for that price");
            return price;    
        }

        public async Task DeleteOnePriceAsync(int id)
        {
            PriceDb price = await appDbContext.price.SingleOrDefaultAsync(p => p.id == id);
            if (price == null)
                throw new KeyNotFoundException("Price not found");

            appDbContext.price.Remove(price);
            await appDbContext.SaveChangesAsync();
        }
    }
}
