using AutoMapper;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{

    public class AddClientOrderHandler : ICommandHandler<AddClientOrder>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
      
       

        public AddClientOrderHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(AddClientOrder command)
        {
            var order = _mapper.Map<Order>(command.Order);
            order.OrderID = Guid.NewGuid();
            order.UserID = Guid.Parse(command.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _orderRepository.AddAsync(order);
        }
    }
}
