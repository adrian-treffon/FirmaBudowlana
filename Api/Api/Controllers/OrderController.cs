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

        public OrderController(IMapper mapper, IOrderRepository orderRepository,IWorkerRepository workerRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
        }

        [HttpGet]
        public IActionResult AddByClient()
        {
            return Ok();
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
        public async Task<IActionResult> ShowUnvalidated()
        => Ok(await _orderRepository.GetAllInvalidatedAsync());


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Validate(Guid orderID)
        {
            var order = await _orderRepository.GetAsync(orderID);
            var workers = await _workerRepository.GetAllAsync();

            var dto = _mapper.Map<AdminOrderDTO>(order);
            dto.Workers = workers;

            return Ok(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Validate([FromBody]AdminOrderDTO adminOrder)
        {
            var order = _mapper.Map<Order>(adminOrder);

            var team = new Team()
            {
                Description = adminOrder.Description,
                TeamID = Guid.NewGuid()
            };

            foreach (var worker in adminOrder.Workers)
            {
                team.WorkersTeams.Add(new WorkerTeam
                {
                    Team = team,
                    Worker = worker
                });
            }
           
            
            order.Validated = true;
            order.OrdersTeams = new List<OrderTeam>
            {
              new OrderTeam {
               Order = order,
               Team = team
              }
            };

            await _orderRepository.UpdateAsync(order);
            return Ok();
        }


    }
}