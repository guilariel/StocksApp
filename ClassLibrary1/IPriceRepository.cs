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
    internal interface IPriceRepository : IRepository<PriceDb>
    {

    }
    public class PriceRepository : EFRepository<PriceDb> , IPriceRepository
    {
        public PriceRepository(DbContext context) : base(context)
        {
        }
    }
}
