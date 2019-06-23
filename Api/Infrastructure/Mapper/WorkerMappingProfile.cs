using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using System.Linq;

namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class WorkerMappingProfile : IMappingAction<Worker, WorkerDTO>
    {
        private readonly ITeamRepository _teamRepository;

        public WorkerMappingProfile(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public void Process(Worker source, WorkerDTO destination)
        {
            if (source.WorkerTeam == null) return;

            var teamIDs = source.WorkerTeam.Where(x => x.WorkerID == destination.WorkerID).Select(y => y.TeamID);

            foreach (var ID in teamIDs)
            {
                var team = _teamRepository.GetAsync(ID).Result;
                destination.Teams.Add(team);

            }
        }
    }
}
