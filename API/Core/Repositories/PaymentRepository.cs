using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DBContext _context;

        public PaymentRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetAsync(Guid id)
            => await _context.Payments.SingleOrDefaultAsync(x => x.PaymentID == id);

        public async Task<IEnumerable<Payment>> GetAllAsync()
            => await _context.Payments.ToListAsync();

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var payment = await GetAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
