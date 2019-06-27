using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.User;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.User
{
    public class GetUserSpecifyOrderHandler : ICommandHandler<GetUserSpecifyOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetUserSpecifyOrderHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task HandleAsync(GetUserSpecifyOrder command)
        {
          
            if (Guid.Parse(command.User.FindFirst(ClaimTypes.NameIdentifier).Value) != command.UserID)
                throw new ServiceException(ErrorCodes.NiepoprawnyFormat,"Niepoprawny token");
           
            var order = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == command.UserID).SingleOrDefault(x => x.OrderID == command.OrderID);

            if (order == null)
               throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć zlecenia w bazie danych");

           command.Order = _mapper.Map<AdminOrderDTO>(order);
        }
    }
}
