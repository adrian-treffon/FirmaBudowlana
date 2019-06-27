using AutoMapper;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class ValidateOrderHandler : ICommandHandler<ValidateOrder>
    {

        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;

        public ValidateOrderHandler(IMapper mapper, IOrderRepository orderRepository,DBContext context)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _context = context;
        }
        public async Task HandleAsync(ValidateOrder command)
        {
            var _order = await _orderRepository.GetAsync(command.Order.OrderID);

            if (_order == null) throw new ServiceException(ErrorCodes.Nieznaleziono,"Nie znaleziono zlecenia w bazie danych");

            var order = _mapper.Map<Order>(command.Order);
            order.Validated = true;

            if (!command.Order.Teams.Any()) throw new ServiceException(ErrorCodes.NiepoprawnyFormat,"Wybierz przynajmniej jeden zespół");

            foreach (var team in command.Order.Teams)
            {
                order.OrderTeam.Add(
                    new OrderTeam
                    {
                        OrderID = order.OrderID,
                        TeamID = team.TeamID
                    }
                    );
            }

            await _context.OrderTeam.AddRangeAsync(order.OrderTeam);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
