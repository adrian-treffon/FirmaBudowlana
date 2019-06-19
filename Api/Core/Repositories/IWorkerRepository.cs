using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.Repositories
{
    public interface IWorkerRepository :IRepository
    {
        Task<Worker> GetAsync(Guid id);
        Task<IEnumerable<Worker>> GetAllAsync();
        Task AddAsync(Worker Worker);
        Task UpdateAsync(Worker Worker);
        Task RemoveAsync(Worker worker);
        Task<IEnumerable<Worker>> GetAllActiveAsync();
    }
}
