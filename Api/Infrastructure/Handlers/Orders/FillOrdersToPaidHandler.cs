using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Extensions;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class FillOrdersToPaidHandler : ICommandHandler<FillOrdersToPaid>
    {
        private readonly ITeamRepository _teamRepository;
      
        private readonly DBContext _context;
    
        private readonly IMapper _mapper;

        public FillOrdersToPaidHandler(ITeamRepository teamRepository,DBContext context,IMapper mapper)
        {
            _teamRepository = teamRepository;
            _context = context;
            _mapper = mapper;
        }


        public async Task HandleAsync(FillOrdersToPaid command)
        {
            foreach (var order in command.Orders)
            {
                var teams = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();

                if (teams == null) throw new Exception($"There is no team to pay");

                var days = DaysWithoutWeekends.Count(order.StartDate, order.EndDate);


                foreach (var teamID in teams)
                {
                    var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(teamID.TeamID));

                    if (team == null) throw new Exception($"Cannot find the team {team.TeamID} in DB");


                    foreach (var worker in team.Workers)
                    { order.PaymentCost += worker.ManHour * 8 * days; }

                    order.Teams.Add(team);
                }
            }
        }
    }
}
