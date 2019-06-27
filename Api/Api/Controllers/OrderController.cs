using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirmaBudowlana.Api.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderController(IOrderRepository orderRepository,ICommandDispatcher commandDispatcher, IMapper mapper)
        { 
            _orderRepository = orderRepository;
            _commandDispatcher = commandDispatcher;
            _mapper= mapper;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddByClient([FromBody]ClientOrderDTO clOrder)
        {
            var command = new AddClientOrder()
            {
                Order = clOrder,
                User = User
            };

           
           await _commandDispatcher.DispatchAsync(command);

           return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShowInvalidated()
        {
            var orders = _mapper.Map<IEnumerable<ComparisonOrderDTO>>(await _orderRepository.GetAllInvalidatedAsync());
            return new JsonResult(orders);
        } 


       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Validate(Guid id)
        {
            var command = new GetInvalidatedOrder() {OrderID = id };

            await _commandDispatcher.DispatchAsync(command);

            return new JsonResult(command.Order);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Validate([FromBody]AdminOrderDTO adminOrder)
        {
            
            await _commandDispatcher.DispatchAsync(new ValidateOrder() { Order = adminOrder});
          
            return Ok();
        }
    }
}