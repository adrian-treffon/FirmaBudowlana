using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        {
             var worker = await _context.Workers.AsNoTracking().SingleOrDefaultAsync(x => x.WorkerID == id);

             worker = await GetWorkerTeamList(worker);

            return worker;
        }


        public async Task<IEnumerable<Worker>> GetAllAsync()
        {
            var workers = await _context.Workers.ToListAsync();

            for (int i = 0; i < workers.Count(); i++)
            {
                workers[i] = await GetWorkerTeamList(workers[i]);
            }

            return workers;

        }

        public async Task<IEnumerable<Worker>> GetAllActiveAsync()
        {
            var workers = await _context.Workers.Where(x => x.Active == true).ToListAsync();

            for(int i =0; i <workers.Count();i++)
            {
                workers[i] = await GetWorkerTeamList(workers[i]);
            }

            return workers;
        }
       

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

        private async Task<Worker> GetWorkerTeamList(Worker worker)
        {
            worker.WorkerTeam = await _context.WorkerTeam.Where(x => x.WorkerID == worker.WorkerID).ToListAsync();

            foreach (var workerteam in worker.WorkerTeam)
            {
                workerteam.Team = await _context.Teams.Where(x => x.TeamID == workerteam.TeamID).SingleAsync();
            }

            return worker;
        }
      
    }
}
