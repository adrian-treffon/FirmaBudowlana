﻿using System;
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
            => await _context.Teams.SingleOrDefaultAsync(x => x.TeamID == id);


        public async Task<IEnumerable<Team>> GetAllAsync()
            => await _context.Teams.ToListAsync();

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var team = await GetAsync(id);
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