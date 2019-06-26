using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirmaBudowlana.Api.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderController(IMapper mapper, IOrderRepository orderRepository, ITeamRepository teamRepository,
             ICommandDispatcher commandDispatcher)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _teamRepository = teamRepository;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddByClient([FromBody]ClientOrderDTO clOrder)
        {
            var order = _mapper.Map<Order>(clOrder);
            order.OrderID = Guid.NewGuid();
            order.UserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _orderRepository.AddAsync(order);
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
            if(id == Guid.Empty) return BadRequest(new { message = "Incorrect ID format" });
           
            var order = await _orderRepository.GetAsync(id);
            if (order == null) return NotFound(new { message = "Order not found" });

            var teams = await _teamRepository.GetAllAsync();

            var dto = _mapper.Map<AdminOrderDTO>(order);
            dto.Teams = teams;

            return new JsonResult(dto);
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