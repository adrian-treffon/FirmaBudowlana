using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.EF;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class CreateReportHandler : ICommandHandler<CreateReport>
    {

        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;

        public CreateReportHandler(IMapper mapper, IOrderRepository orderRepository, DBContext context)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task HandleAsync(CreateReport command)
        {

            if (command.Report.Teams.Any() || command.Report.Workers.Any())
            {
                var ordersID = new List<Guid>();

                if (command.Report.Teams.Any() && !command.Report.Workers.Any())
                {
                    foreach (var team in command.Report.Teams)
                    {
                        ordersID.AddRange(_context.OrderTeam.Where(x => x.TeamID == team.TeamID).Select(x => x.OrderID).ToList());
                    }

                }
                else if (!command.Report.Teams.Any() && command.Report.Workers.Any())
                {
                    var teamIDs = new List<Guid>();

                    foreach (var worker in command.Report.Workers)
                    {
                        teamIDs.AddRange(_context.WorkerTeam.Where(x => x.WorkerID == worker.WorkerID).Select(x => x.TeamID).ToList());
                    }

                    foreach (var teamID in teamIDs)
                    {
                        ordersID.AddRange(_context.OrderTeam.Where(x => x.TeamID == teamID).Select(o => o.OrderID).ToList());
                    }

                }
                else if (command.Report.Teams.Any() && command.Report.Workers.Any()) throw new Exception($"You can only choose teams or workers, not both");

                foreach (var id in ordersID)
                {
                    var order = _mapper.Map<ComparisonOrderDTO>(await _orderRepository.GetAsync(id));

                    if (!command.Orders.Select(o => o.OrderID).Contains(order.OrderID))
                    {
                        command.Orders.Add(order);
                    }

                }
            }
            else if (!command.Report.Teams.Any() || !command.Report.Workers.Any())
            {
                command.Orders = _mapper.Map<List<ComparisonOrderDTO>>(await _orderRepository.GetAllValidatedAsync());
            }

            if (command.Report.StartDate != null && command.Report.EndDate != null)
                command.Orders = command.Orders.Where(x => x.StartDate >= command.Report.StartDate && x.StartDate <= command.Report.EndDate).ToList();

            command.Orders = command.Orders.OrderBy(x => x.StartDate).ToList();
        }
    }
}
