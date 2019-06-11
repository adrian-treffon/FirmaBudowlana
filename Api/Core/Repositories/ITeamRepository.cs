using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.Repositories
{
    public interface ITeamRepository :IRepository
    {
        Task<Team> GetAsync(Guid id);
        Task<IEnumerable<Team>> GetAllAsync();
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task RemoveAsync(Team team);
    }
}
