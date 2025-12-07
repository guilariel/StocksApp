using Microsoft.EntityFrameworkCore;
using SellStocks.Domain.Entities;

namespace PurchaseStocks.Infrastructure
{
    public class CrudStocks
    {
        private readonly AppDbContext _context;

        public CrudStocks(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos
        public async Task<List<StockDb>> GetAllAsync()
        {
            return await _context.stock.AsNoTracking().ToListAsync();
        }

        // Obtener uno por ID
        public async Task<StockDb?> GetByIdAsync(int id)
        {
            return await _context.stock.AsNoTracking().SingleOrDefaultAsync(s => s.id == id);
        }
        public async Task<StockDb?> GetOneByNameAsync(string name)
        {
            return await _context.stock.AsNoTracking().SingleOrDefaultAsync(s => s.name == name);
        }
        // Añadir nuevo
        public async Task AddAsync(string symbol, string name, string description, int price_id)
        {
            StockDb stock = new StockDb
            {
                symbol = symbol,
                name = name,
                description = description,
                price_id = price_id
            };
            _context.stock.Add(stock);
            await _context.SaveChangesAsync();
        }

        // Actualizar
        public async Task UpdateAsync(int id, string symbol, string name, string description, int price_id)
        {
            var existing = await _context.stock.FirstOrDefaultAsync(s => s.id == id);
            if (existing == null)
                throw new KeyNotFoundException("Stock not found.");

            existing.symbol = symbol;
            existing.name = name;
            existing.description = description;
            existing.price_id = price_id;

            await _context.SaveChangesAsync();
        }

        // Eliminar
        public async Task DeleteAsync(int id)
        {
            var stock = await _context.stock.FindAsync(id);
            if (stock == null)
                throw new KeyNotFoundException("Stock not found.");

            _context.stock.Remove(stock);
            await _context.SaveChangesAsync();
        }
        public async Task<PriceDb?> GetStockPriceByNameAsync(string name)
        {
            StockDb? stockDb = await _context.stock.AsNoTracking().SingleOrDefaultAsync(s =>s.name == name);
            return await _context.price.AsNoTracking().SingleOrDefaultAsync(p => p.id == stockDb.price_id);
        }
    }
}
