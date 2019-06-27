using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Team;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Commands.Worker
{
    public class DeleteWorkerHandler : ICommandHandler<DeleteWorker>
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteWorkerHandler(IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository, DBContext context, ICommandDispatcher commandDispatcher)
        {
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _context = context;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(DeleteWorker command)
        {
            var worker = await _workerRepository.GetAsync(command.WorkerID);

            if (worker == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć pracownika w bazie danych");

            if (worker.Active == false) throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można zwolnić pracownika, gdyż został już zwolniony wcześniej");

            var teamIDs = _context.WorkerTeam.Where(x => x.WorkerID == command.WorkerID).Select(y => y.TeamID);

            foreach (var teamID in teamIDs)
            {
                var ordersID = _context.OrderTeam.Where(x => x.TeamID == teamID).Select(y => y.OrderID);

                foreach (var orderID in ordersID)
                {
                    var order = await _orderRepository.GetAsync(orderID);
                    if (!order.Paid)
                        throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można zwolnić pracownika, ponieważ pracuje on w przynajmniej jednym aktywnym zleceniu");
                }

            }

            worker.Active = false;

            await _workerRepository.UpdateAsync(worker);

            await DeleteEmptyTeam();
        }

        private async Task DeleteEmptyTeam()
        {
            var workerteam = await _context.WorkerTeam.ToListAsync();
            var teams = await _teamRepository.GetAllActiveAsync();
            var workers = await _workerRepository.GetAllActiveAsync();

            foreach (var team in teams)
            {
                var workersInTeam = workerteam.Where(x => x.TeamID == team.TeamID).Select(c => c.WorkerID).ToList();
                int inactiveWorkers = 0;
                foreach (var workerID in workersInTeam)
                {
                    var worker = workers.Where(x => x.WorkerID == workerID).SingleOrDefault();
                    if (worker.Active == false) inactiveWorkers++;
                }
                if (inactiveWorkers == workersInTeam.Count() || workersInTeam.Count() == 0)
                {
                    await _commandDispatcher.DispatchAsync(new DeleteTeam() { TeamID = team.TeamID });
                }
            }

        }
    }
}
