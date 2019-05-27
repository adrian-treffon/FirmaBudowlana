using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.Repositories
{
    public interface IOrderRepository :IRepository
    {
        Task<Order> GetAsync(Guid id);
        Task<IEnumerable<Order>> GetAllInvalidatedAsync();
        Task<IEnumerable<Order>> GetAllValidatedAsync();
        Task<IEnumerable<Order>> GetAllUnpaidAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task RemoveAsync(Guid id);
    }
}
