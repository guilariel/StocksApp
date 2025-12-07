using Microsoft.EntityFrameworkCore;
using SellStocks.Domain.Entities;
namespace PurchaseStocks.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<InPossessionDb> in_possession { get; set; }
        public DbSet<PriceDb> price { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
