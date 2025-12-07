using Microsoft.EntityFrameworkCore;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;

namespace PurchaseStocks.Infrastructure
{
    public class CrudPossession
    {
        private readonly AppDbContext _context;

        public CrudPossession(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos
        public async Task<List<InPossessionDb>> GetAllAsync()
        {
            return await _context.in_possession.AsNoTracking().ToListAsync();
        }

        // Obtener uno específico (por owner_id y symbol_id)
        public async Task<InPossessionDb?> GetOneAsync(int ownerId, int symbolId)
        {
            return await _context.in_possession.AsNoTracking().SingleOrDefaultAsync(
                i => i.owner_id == ownerId && i.stock_id == symbolId
            );
        }

        public async Task<List<InPossessionDb>> GetPossessionsByOwnerId(int ownerId)
        {
            return await _context.in_possession.AsNoTracking()
                .Where(i => i.owner_id == ownerId)
                .ToListAsync();
        }

        // Eliminar registro
        public async Task DeleteAsync(InPossessionDb possession)
        {

            if (possession == null)
                throw new KeyNotFoundException("Possession not found.");

            _context.in_possession.Remove(possession);
            await _context.SaveChangesAsync();
        }

    }
}


//public async Task<List<InPossessionDb>> GetPossessionsByOwner(string ownerName,CrudUsers crudUsers)
//{
//    List<InPossessionDb> possessions = await GetAllAsync();
//    UsersDb? owner = await crudUsers.GetOneByNameAsync(ownerName);
//    return await _context.in_possession.Where(i => i.owner_id == owner.id).ToListAsync();
//}