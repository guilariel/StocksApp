using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseDll
{
    interface IPriceRepository : IRepository<PriceHistoryDb, int>
    {
        Task<PriceHistoryDb?> GetLatestAsync();
        Task<PriceHistoryDb?> GetByPriceIdAsync(int id);
    }
    public class PriceRepository : EFRepository<PriceHistoryDb, int>, IPriceRepository
    {
        public PriceRepository(DbContext context) : base(context)
        {
        }
        public async Task<PriceHistoryDb?> GetLatestAsync()
        {
            return await _entities
                .OrderByDescending(e => e.date)
                .FirstOrDefaultAsync();
        }
        public async Task<PriceHistoryDb?> GetByPriceIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.stock_id == id);
        }
    }
}
