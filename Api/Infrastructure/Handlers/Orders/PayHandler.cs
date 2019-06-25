using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Extensions;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class PayHandler : ICommandHandler<OrderToPaidDTO>
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;
        private readonly IPaymentRepository _paymentRepository;

        public PayHandler(IWorkerRepository workerRepository, ITeamRepository teamRepository, IOrderRepository orderRepository, 
            DBContext context, IPaymentRepository paymentRepository)
        {
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _context = context;
            _paymentRepository = paymentRepository;
        }

        public async Task HandleAsync(OrderToPaidDTO command)
        {
            if (command == null) throw new Exception("Post request add/payment is empty");

            var order = await _orderRepository.GetAsync(command.OrderID);

            if (order == null) throw new Exception($"Cannot find the order {command.OrderID} in DB");

            foreach (var teamID in command.Teams)
            {
                var team = await _teamRepository.GetAsync(teamID.TeamID);

                if (team == null) throw new Exception($"There is no team {team.TeamID} to pay");

                var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();

                if (!workers.Any()) throw new Exception($"There is no workers in the team {team.TeamID} to pay");

                foreach (var ele in workers)
                {
                    var worker = await _workerRepository.GetAsync(ele.WorkerID);
                    var days = DaysWithoutWeekends.Count(order.StartDate, (DateTime)order.EndDate);

                    var payment = new Payment
                    {
                        OrderID = command.OrderID,
                        WorkerID = worker.WorkerID,
                        Amount = worker.ManHour * 8 * days,
                        PaymentDate = DateTime.UtcNow,
                        PaymentID = Guid.NewGuid()
                    };

                    await _paymentRepository.AddAsync(payment);
                }
            }

            order.Paid = true;
            await _orderRepository.UpdateAsync(order);
        }

    }
}
