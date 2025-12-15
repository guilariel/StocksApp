using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    internal interface IInPossessionRepository : IRepository<InPossessionDb>
    {
        Task<List<InPossessionDb>> GetPossessionsByOwnerId(int ownerId);
        Task DeleteAsync(int ownerId, int symbolId);

    }
    public class InPossessionRepository : EFRepository<InPossessionDb> , IInPossessionRepository
    {
        public InPossessionRepository(DbContext context) : base(context)
        {
        }
        public async Task<InPossessionDb?> GetByIdAsync(int ownerId, int symbolId)
        {
            return await _entities.AsNoTracking().SingleOrDefaultAsync(
                i => i.id == ownerId && i.stock_id == symbolId
            );
        }
        public async Task<List<InPossessionDb>> GetPossessionsByOwnerId(int ownerId)
        {
            return await _entities.AsNoTracking()
                .Where(i => i.id == ownerId)
                .ToListAsync();
        }
        public async Task DeleteAsync(int ownerId, int symbolId)
        {
            var possession = await _entities.AsNoTracking().SingleOrDefaultAsync(
                i => i.id == ownerId && i.stock_id == symbolId
            );

            if (possession == null)
                throw new KeyNotFoundException("Possession not found.");

            _entities.Remove(possession);
            await _context.SaveChangesAsync();
        }
    }
}
