using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellDll
{
    public interface IUserRepository : IRepository<UsersDb>
    {
        public Task<UsersDb?> GetOneByNameAsync(string name);
        public Task AddFundsAsync(int id, double cost);
        public Task SellFundsAsync(int id, double cost);
    }
    public class UserRepository : EFRepository<UsersDb>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
        public async Task<UsersDb?> GetOneByNameAsync(string name)
        {
            UsersDb? user = _entities.FirstOrDefault(u => u.name == name);
            return await Task.FromResult(user);
        }
        public async Task AddFundsAsync(int id, double cost)
        {
            UsersDb? user = _entities.FirstOrDefault(u => u.id == id);
            user.funds += cost;
            await Task.CompletedTask;
        }
        public async Task SellFundsAsync(int id, double cost)
        {
            UsersDb? user = _entities.FirstOrDefault(u => u.id == id);
            user.funds -= cost;
            await Task.CompletedTask;
        }
    }
}
