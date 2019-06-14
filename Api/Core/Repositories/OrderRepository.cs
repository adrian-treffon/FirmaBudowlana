using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace FirmaBudowlana.Core.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DBContext _context;

        public OrderRepository(DBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _context.Orders.ToListAsync();
            
        public async Task<IEnumerable<Order>> GetAllInvalidatedAsync()
         => await _context.Orders.Where(x => x.Validated == false).ToListAsync();

        public async Task<IEnumerable<Order>> GetAllUnpaidAsync()
             => await _context.Orders.Where(x => x.Paid== false && x.Validated == true).ToListAsync();


        public async Task<IEnumerable<Order>> GetAllValidatedAsync()
        => await _context.Orders.Where(x => x.Validated == true).ToListAsync();

        public async Task<Order> GetAsync(Guid id)
        => await _context.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.OrderID == id);

        public async Task RemoveAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
