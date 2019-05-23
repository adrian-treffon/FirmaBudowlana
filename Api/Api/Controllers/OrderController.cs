using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
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
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;

        public OrderController(IMapper mapper, IOrderRepository orderRepository,IWorkerRepository workerRepository, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddByClient([FromBody]ClientOrderDTO clOrder)
        {
            var order = _mapper.Map<Order>(clOrder);
            order.OrderID = Guid.NewGuid();
            await _orderRepository.AddAsync(order);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ShowInvalidated()
        => Ok(await _orderRepository.GetAllInvalidatedAsync());


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Validate([FromBody]Guid orderID)
        {
            var order = await _orderRepository.GetAsync(orderID);
            var teams = await _teamRepository.GetAllAsync();

            var dto = _mapper.Map<AdminOrderDTO>(order);
            dto.Teams = teams;

            return new JsonResult(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Validate([FromBody]AdminOrderDTO adminOrder)
        {
            var _order = await _orderRepository.GetAsync(adminOrder.OrderID);
            if (_order == null) return NotFound(new { message = "Order not found" });

            var order = _mapper.Map<Order>(adminOrder);
            order.Validated = true;


            order.OrdersTeams = new List<OrderTeam>();

            foreach (var team in adminOrder.Teams)
            {
                order.OrdersTeams.Add( 
                    new OrderTeam
                    {
                        Order = order,
                        OrderID = order.OrderID,
                        Team= team,
                        TeamID = team.TeamID
                    }
                    );
            }
           
            await _orderRepository.UpdateAsync(order);
            return Ok();
        }


    }
}