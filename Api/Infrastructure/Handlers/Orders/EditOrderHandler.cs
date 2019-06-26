using AutoMapper;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.EF;
using Komis.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            if (command.Order == null)throw new Exception("Post request edit/order is empty");

            var orderFromDB = await _orderRepository.GetAsync(command.Order.OrderID);

            if (orderFromDB == null) throw new Exception($"Cannot find the order {command.Order.OrderID} in DB");

            var order = _mapper.Map<Order>(command.Order);

            if (orderFromDB.Validated == false) throw new Exception($"You cannot edit invalidated order");

            if (orderFromDB.Paid == true) throw new Exception($"You cannot edit paid/finished order");


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

            var orderTeam = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();
            _context.OrderTeam.RemoveRange(orderTeam);

            await _context.OrderTeam.AddRangeAsync(order.OrderTeam);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
