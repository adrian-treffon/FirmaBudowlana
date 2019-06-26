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
        private readonly IOrderRepository _orderRepository;
      
        public FillOrdersToPaidHandler(ITeamRepository teamRepository,DBContext context,IMapper mapper, IOrderRepository orderRepository)
        {
            _teamRepository = teamRepository;
            _context = context;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }


        public async Task HandleAsync(FillOrdersToPaid command)
        {
            command.Orders = _mapper.Map<IEnumerable<OrderToPaidDTO>>((await _orderRepository.GetAllUnpaidAsync()).ToList());

            foreach (var order in command.Orders)
            {
                var teams = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();

                if (teams == null) throw new Exception($"Nie znaleziono zespołów");

                var days = DaysWithoutWeekends.Count(order.StartDate, order.EndDate);

                foreach (var teamID in teams)
                {
                    var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(teamID.TeamID));

                    if (team == null) throw new Exception($"Nie znaleziono zespołu w bazie danych");


                    foreach (var worker in team.Workers)
                    { order.PaymentCost += worker.ManHour * 8 * days; }

                    order.Teams.Add(team);
                }
            }
        }
    }
}
