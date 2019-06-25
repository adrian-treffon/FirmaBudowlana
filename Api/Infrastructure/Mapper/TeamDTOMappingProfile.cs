using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using System.Linq;


namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class TeamDTOMappingProfile : IMappingAction<Team, TeamDTO>
    {
        public void Process(Team source, TeamDTO destination)
        {
            if (source.WorkerTeam == null) return;

            var workers = source.WorkerTeam.Where(x => x.TeamID == destination.TeamID).Select(y => y.Worker);

            foreach (var worker in workers)
            {
                destination.Workers.Add(worker);
            }
        }
    }
}
