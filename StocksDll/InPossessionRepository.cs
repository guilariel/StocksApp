using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDll
{
    internal interface IInPossessionRepository : IRepository<InPossessionDb>
    {
        Task<List<InPossessionDb>> GetAllByOwnerAsync(string owner_id);
    }
    public class InPossessionRepository : EFRepository<InPossessionDb>, IInPossessionRepository
    {
        public InPossessionRepository(DbContext context) : base(context)
        {
        }
        public async Task<List<InPossessionDb>> GetAllByOwnerAsync(string owner_id)
        {
            return await _entities.Where(id => id.owner_id == owner_id).ToListAsync();
        }
    }
}
