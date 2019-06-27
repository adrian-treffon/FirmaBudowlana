using AutoMapper;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.EF;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class EditOrderHandler : ICommandHandler<EditOrder>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;
       

        public EditOrderHandler(IOrderRepository orderRepository, IMapper mapper, DBContext context)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task HandleAsync(EditOrder command)
        {
            if (command.Order == null)throw new ServiceException(ErrorCodes.PustyRequest,"Post request edit/order is empty");

            var orderFromDB = await _orderRepository.GetAsync(command.Order.OrderID);

            if (orderFromDB == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć zlecenia w bazie danych");

            var order = _mapper.Map<Order>(command.Order);

            if (orderFromDB.Validated == false || orderFromDB.Paid == true)
                throw new ServiceException(ErrorCodes.BladEdycji,$"Nie można edytować zakończonych zleceń");

      
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

            order.Validated = true;
            order.UserID = orderFromDB.UserID;

            var orderTeam = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();
            _context.OrderTeam.RemoveRange(orderTeam);

            await _context.OrderTeam.AddRangeAsync(order.OrderTeam);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
