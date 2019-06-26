using AutoMapper;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Team;
using FirmaBudowlana.Infrastructure.EF;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Team
{
    public class EditTeamHandler : ICommandHandler<EditTeam>
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;

        public EditTeamHandler(ITeamRepository teamRepository, IOrderRepository orderRepository, IMapper mapper, DBContext context)
        {
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task HandleAsync(EditTeam command)
        {
            if (command.Team == null) throw new Exception("Post request edit/team is empty");

            var teamFromDB = await _teamRepository.GetAsync(command.Team.TeamID);

            if (teamFromDB == null) throw new Exception("Nie można znaleźć zespołu w bazie");

            var orderTeam = _context.OrderTeam.Where(x => x.TeamID == command.Team.TeamID).Select(o => o.OrderID);

            foreach (var orderID in orderTeam)
            {
                var order = await _orderRepository.GetAsync(orderID);
                if (!order.Paid) throw new Exception("Nie można edytować zespołu, który obecnie posiada aktywne zlecenia");
            }

            var team = _mapper.Map<Core.Models.Team>(command.Team);

            var workerTeam = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
            _context.WorkerTeam.RemoveRange(workerTeam);

            await _context.WorkerTeam.AddRangeAsync(team.WorkerTeam);
            await _teamRepository.UpdateAsync(team);
        }
    }
}
