using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.DbEntities;
namespace StocksApp.Infrastructure.ExternalServices
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
