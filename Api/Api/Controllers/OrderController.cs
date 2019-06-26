using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FirmaBudowlana.Api.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderController(IOrderRepository orderRepository,ICommandDispatcher commandDispatcher)
        {
            _orderRepository = orderRepository;
            _commandDispatcher = commandDispatcher;
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

            try
            {
                await _commandDispatcher.DispatchAsync(command);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShowInvalidated()
        => new JsonResult(await _orderRepository.GetAllInvalidatedAsync());


       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Validate(Guid id)
        {
            var command = new GetInvalidatedOrder() {OrderID = id };

            try
            {
                await _commandDispatcher.DispatchAsync(command);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return new JsonResult(command.Order);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Validate([FromBody]AdminOrderDTO adminOrder)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new ValidateOrder() { Order = adminOrder});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return Ok();
        }
    }
}