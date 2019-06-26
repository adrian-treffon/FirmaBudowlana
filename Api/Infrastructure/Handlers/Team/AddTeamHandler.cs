using AutoMapper;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Team;
using Komis.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Team
{
    public class AddTeamHandler : ICommandHandler<AddTeam>
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;
    

        public AddTeamHandler(IMapper mapper, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public async Task HandleAsync(AddTeam command)
        {
            if (command.Team== null)throw new Exception( "Post request add/team is empty");
            if (command.Team.Workers.Count == 0) throw new Exception("Wybierz przynajmnniej jedego pracownika");
            var team = _mapper.Map<Core.Models.Team>(command.Team);
            team.TeamID = Guid.NewGuid();
            await _teamRepository.AddAsync(team);
        }
    }
}
