using Microsoft.EntityFrameworkCore;
using LogIn.Domain.Entities;
namespace LogIn.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<UsersDb> users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
