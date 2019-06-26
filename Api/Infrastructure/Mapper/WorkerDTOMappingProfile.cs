using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using System.Linq;

namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class WorkerDTOMappingProfile : IMappingAction<Worker, WorkerDTO>
    {
        
        public void Process(Worker source, WorkerDTO destination)
        {
            if (source.WorkerTeam == null) return;

            var teams = source.WorkerTeam.Where(x => x.WorkerID == destination.WorkerID).Select(y => y.Team);

            foreach (var team in teams)
            {
                destination.Teams.Add(team);
            }
        }
    }
}
