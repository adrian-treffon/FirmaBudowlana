using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Order;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ComparisonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public ComparisonController(IMapper mapper, IOrderRepository orderRepository, IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IPaymentRepository paymentRepository, ICommandDispatcher commandDispatcher)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _paymentRepository = paymentRepository;
            _commandDispatcher = commandDispatcher;
        }


        [HttpGet]
        public async Task<IActionResult> Workers()
        {
            var workers = _mapper.Map<IEnumerable<WorkerDTO>>(await _workerRepository.GetAllActiveAsync());

            foreach (var worker in workers)
            {
                foreach (var team in worker.Teams.ToList())
                {
                    if (!team.Active) worker.Teams.Remove(team);
                }                 
            }
            return new JsonResult(workers);
        }
       
            
        [HttpGet]
        public async Task<IActionResult> AllWorkers()
           => new JsonResult(_mapper.Map<IEnumerable<WorkerDTO>>(await _workerRepository.GetAllAsync()));
             
        [HttpGet("Comparison/Workers/{id}")]
        public async Task<IActionResult> Workers(Guid id)
        {
            var worker = _mapper.Map<WorkerDTO>(await _workerRepository.GetAsync(id));

            if (worker == null) return BadRequest(new { message = $"Cannot find the worker {id} in DB" });

            return new JsonResult(worker);
        }

        [HttpGet]
        public async Task<IActionResult> Teams()
        {
            var teams = _mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAllActiveAsync());

            foreach (var team in teams)
            {
                foreach (var worker in team.Workers)
                {
                    if (!worker.Active) team.Workers.Remove(worker);
                }
            }
            return new JsonResult(teams);
        }

        [HttpGet]
        public async Task<IActionResult> AllTeams()
         => new JsonResult(_mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAllAsync()));
      
        [HttpGet("Comparison/Teams/{id}")]
        public async Task<IActionResult> Teams(Guid id)
        {
            var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(id));

            if (team == null) return BadRequest(new { message = $"Cannot find the team {id} in DB" });

            return new JsonResult(team);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = _mapper.Map<IEnumerable<ComparisonOrderDTO>>((await _orderRepository.GetAllValidatedAsync())
                .OrderBy(x => x.StartDate)).ToList();

            return new JsonResult(orders);
        }


        [HttpGet("Comparison/Orders/{id}")]
        public async Task<IActionResult> Orders(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order == null) return BadRequest(new { message = $"Cannot find the order {id} in DB" });

            var fullOrder = _mapper.Map<ComparisonOrderDTO>(order);

            return new JsonResult(fullOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Payments()
        {
            var payments = (await _paymentRepository.GetAllAsync()).OrderByDescending(x => x.PaymentDate).ToList();

            return new JsonResult(payments);
        }

        [HttpGet("Comparison/Payments/{id}")]
        public async Task<IActionResult> Payments(Guid id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            if (payment == null) return BadRequest(new { message = $"Cannot find the payment {id} in DB" });
            return new JsonResult(payment);
        }


        [HttpPost]
        public async Task<IActionResult> Report([FromBody]ReportDTO report)
        {
            var command = new CreateReport() { Report = report };

            try
            {
                await _commandDispatcher.DispatchAsync(command);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
           
            return new JsonResult(command.Orders);
        }
    }
}