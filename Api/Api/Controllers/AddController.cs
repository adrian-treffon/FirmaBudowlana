using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AddController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public AddController(IMapper mapper, IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository, ICommandDispatcher commandDispatcher)
        {
            _mapper = mapper;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {
           if(workerDTO == null) return BadRequest(new { message = "Post request add/worker is empty" });

           var worker =_mapper.Map<Worker>(workerDTO);
           worker.WorkerID = Guid.NewGuid();
           await _workerRepository.AddAsync(worker);
           return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody] TeamDTO teamDTO)
        {
            if (teamDTO == null) return BadRequest(new { message = "Post request add/team is empty" });

            var team = _mapper.Map<Team>(teamDTO);
            team.TeamID = Guid.NewGuid();
            await _teamRepository.AddAsync(team);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromBody]OrderToPaidDTO orderToPaidDTO)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(orderToPaidDTO);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var ordersDTO = _mapper.Map<IEnumerable<OrderToPaidDTO>>((await _orderRepository.GetAllUnpaidAsync()).ToList());

            try
            {
                await _commandDispatcher.DispatchAsync(new FillOrdersToPaid() { Orders = ordersDTO });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return new JsonResult(ordersDTO);
        }

      

    }
}