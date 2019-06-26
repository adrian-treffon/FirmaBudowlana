using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;


namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class TeamMappingProfile : IMappingAction<TeamDTO, Team>
    {
        public void Process(TeamDTO source, Team destination)
        {
            foreach (var worker in source.Workers)
            {
                destination.WorkerTeam.Add(
                    new WorkerTeam()
                    {
                        TeamID = destination.TeamID,
                        WorkerID = worker.WorkerID
                    }
              );
            }
        }
    }
}
