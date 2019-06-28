using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace FirmaBudowlana.Core.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DBContext _context;
      
       public TeamRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Team> GetAsync(Guid id)
        {
            var team = await _context.Teams.AsNoTracking().SingleOrDefaultAsync(x => x.TeamID == id);
            await GetWorkerTeamAndOrderTeamLists(team);
            return team;
        }


        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            var teams = await _context.Teams.AsNoTracking().ToListAsync();

            for (int i = 0; i < teams.Count; i++) await GetWorkerTeamAndOrderTeamLists(teams[i]);
            return teams;
        }

        public async Task<IEnumerable<Team>> GetAllActiveAsync()
        {
            var teams = await _context.Teams.AsNoTracking().Where(x => x.Active == true).ToListAsync();

            for (int i = 0; i < teams.Count; i++) await GetWorkerTeamAndOrderTeamLists(teams[i]);

            return teams;
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Team team)
        { 
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        private async Task<Team> GetWorkerTeamAndOrderTeamLists(Team team)
        {
            team.WorkerTeam = _context.WorkerTeam.Where(x => x.TeamID == team.TeamID).ToList();
            team.OrderTeam = _context.OrderTeam.Where(x => x.TeamID == team.TeamID).ToList();

            foreach (var workerTeam in team.WorkerTeam)
            {
                workerTeam.Worker = await _context.Workers.AsNoTracking().Where(w => w.WorkerID == workerTeam.WorkerID).SingleAsync(); 
            }

            foreach (var orderTeam in team.OrderTeam)
            {
                orderTeam.Order = await _context.Orders.AsNoTracking().Where(o=> o.OrderID == orderTeam.OrderID).SingleAsync();
            }

            return team;
        }


    }
}
