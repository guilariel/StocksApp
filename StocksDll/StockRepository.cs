using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
namespace StocksDll
{
    public interface IStockRepository : IRepository<StockDb>
    {
        Task<StockDb?> GetOneByNameAsync(string name);
    }
    public class StockRepository : EFRepository<StockDb> , IStockRepository
    {
        public StockRepository(DbContext context) : base(context)
        {
        }
        public async Task<StockDb?> GetOneByNameAsync(string name)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(s => s.name == name);
        }
    } 
}
