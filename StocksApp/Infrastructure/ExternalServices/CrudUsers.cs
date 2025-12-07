using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.DbEntities;
namespace StocksApp.Infrastructure.ExternalServices
{
    public class CrudUsers
    {
        private readonly AppDbContext _context;

        public CrudUsers(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<UsersDb>> GetAllAsync()
        {
            return await _context.users.AsNoTracking().ToListAsync();
        }

        // Obtener usuario por ID
        public async Task<UsersDb?> GetByIdAsync(int id)
        {
            return await _context.users.AsNoTracking().SingleOrDefaultAsync(u => u.id == id);
        }
        public async Task<UsersDb?>  GetOneByNameAsync(string name)
        {
            return await _context.users.AsNoTracking().SingleOrDefaultAsync(u => u.name == name);
        }

        // Añadir usuario
        public async Task AddAsync(string name, double funds, string password)
        {
            UsersDb user = new UsersDb
            {
                name = name,
                funds = funds,
                password_hash = password
            };
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Actualizar usuario
        public async Task UpdateFundsAsync(int id, double amount)
        {
            var affected = await _context.users
                .Where(u => u.id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.funds, x => x.funds + amount));

            if (affected == 0)
                throw new KeyNotFoundException("User not found.");
        }

        // Eliminar usuario
        public async Task DeleteAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
