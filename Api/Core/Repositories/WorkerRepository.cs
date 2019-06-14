using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly DBContext _context;

        public WorkerRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Worker> GetAsync(Guid id)
            => await _context.Workers.AsNoTracking().SingleOrDefaultAsync(x => x.WorkerID == id);


        public async Task<IEnumerable<Worker>> GetAllAsync()
            => await _context.Workers.ToListAsync();

        public async Task AddAsync(Worker Worker)
        {
            await _context.Workers.AddAsync(Worker);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Worker worker)
        {
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Worker Worker)
        {
            _context.Workers.Update(Worker);
            await _context.SaveChangesAsync();
        }

      
    }
}
