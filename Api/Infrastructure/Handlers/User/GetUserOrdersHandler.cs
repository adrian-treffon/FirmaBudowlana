using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.User;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.User
{
    public class GetUserOrdersHandler : ICommandHandler<GetUserOrders>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
     
        public GetUserOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task HandleAsync(GetUserOrders command)
        {
            try
            {
                if (Guid.Parse(command.User.FindFirst(ClaimTypes.NameIdentifier).Value) != command.UserID)
                    throw new Exception("Niepoprawny token");
            }
            catch (Exception)
            {
                throw new Exception("Niepoprawny token");
            }

            var orders = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == command.UserID);

            command.Orders = _mapper.Map<IEnumerable<AdminOrderDTO>>(orders);
        }
    }
}
