using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class DeleteOrderHandler : ICommandHandler<DeleteOrder>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(DeleteOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderID);

            if (order == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć zlecenia w bazie danych");

            if (order.Validated == true) throw new ServiceException(ErrorCodes.BladUsuwania,$"Nie można usunąć zatwierdzonego zlecenia");

            await _orderRepository.RemoveAsync(order);
        }
    }
}
