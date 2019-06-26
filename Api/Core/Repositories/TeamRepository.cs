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
            => await _context.Teams.AsNoTracking().SingleOrDefaultAsync(x => x.TeamID == id);


        public async Task<IEnumerable<Team>> GetAllAsync()
            => await _context.Teams.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Team>> GetAllActiveAsync()
            => await _context.Teams.AsNoTracking().Where(x=> x.Active == true).ToListAsync();

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
    }
}
