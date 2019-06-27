using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Orders
{
    public class GetInvalidatedOrderHandler : ICommandHandler<GetInvalidatedOrder>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ITeamRepository _teamRepository;

        public GetInvalidatedOrderHandler(IMapper mapper, IOrderRepository orderRepository, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _teamRepository = teamRepository;
        }

        public async Task HandleAsync(GetInvalidatedOrder command)
        {
            if (command.OrderID == Guid.Empty) throw new ServiceException(ErrorCodes.NiepoprawnyFormat,"Niepoprawny format ID");

            var order = await _orderRepository.GetAsync(command.OrderID);
            if (order == null) throw new ServiceException(ErrorCodes.Nieznaleziono, $"Nie znaleziono zlecenia w bazie danych");

            var teams = await _teamRepository.GetAllAsync();

            command.Order = _mapper.Map<AdminOrderDTO>(order);
            command.Order.Teams = teams;
        }
    }
}
