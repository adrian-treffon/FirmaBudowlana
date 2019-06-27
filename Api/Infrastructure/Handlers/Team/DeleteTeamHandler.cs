using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Commands.Team
{
    public class DeleteTeamHandler : ICommandHandler<DeleteTeam>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;
      
        public DeleteTeamHandler(ITeamRepository teamRepository, IOrderRepository orderRepository, DBContext context)
        {
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task HandleAsync(DeleteTeam command)
        {
            var team = await _teamRepository.GetAsync(command.TeamID);

            if (team == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie znalezniono zespołu w bazie danych");

            if (team.Active == false) throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można rozwiązać zespołu, ponieważ został już rozwiązany wcześniej");

            var orderTeam = _context.OrderTeam.Where(x => x.TeamID == team.TeamID).Select(o => o.OrderID).ToList();

            foreach (var orderID in orderTeam)
            {
                var order = await _orderRepository.GetAsync(orderID);
                if (!order.Paid) throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można rozwiązać zespołu, który obecnie posiada aktywne zlecenia");
            }

            team.Active = false;

            await _teamRepository.UpdateAsync(team);
        }
        
    }
}
