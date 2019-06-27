using FirmaBudowlana.Core.Repositories;
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
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;
       

        public DeleteWorkerHandler(IWorkerRepository workerRepository,
             IOrderRepository orderRepository, DBContext context)
        {
            _workerRepository = workerRepository;
          
            _orderRepository = orderRepository;
            _context = context;
          
        }

        public async Task HandleAsync(DeleteWorker command)
        {
            var worker = await _workerRepository.GetAsync(command.WorkerID);

            if (worker == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć pracownika w bazie danych");

            if (worker.Active == false) throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można zwolnić pracownika, gdyż został już zwolniony wcześniej");

            var teamIDs = _context.WorkerTeam.AsNoTracking().Where(x => x.WorkerID == command.WorkerID).Select(y => y.TeamID).ToList();

            foreach (var teamID in teamIDs)
            {
                var ordersID = _context.OrderTeam.AsNoTracking().Where(x => x.TeamID == teamID).Select(y => y.OrderID).ToList();

                foreach (var orderID in ordersID)
                {
                    var order = await _orderRepository.GetAsync(orderID);
                    if (!order.Paid)
                        throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można zwolnić pracownika, ponieważ pracuje on w przynajmniej jednym aktywnym zleceniu");
                }

            }

            worker.Active = false;

            await _workerRepository.UpdateAsync(worker);

        }

    }
}
