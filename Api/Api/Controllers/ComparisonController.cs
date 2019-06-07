using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly DBContext _context;

        public ComparisonController(IMapper mapper, IOrderRepository orderRepository, IWorkerRepository workerRepository, 
            ITeamRepository teamRepository, IPaymentRepository paymentRepository, DBContext context)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _paymentRepository = paymentRepository;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Workers()
        {
            var workers = _mapper.Map<IEnumerable<WorkerDTO>>(await _workerRepository.GetAllAsync());
            return new JsonResult(workers);
        }

        [HttpGet("Comparison/Workers/{id}")]
        public async Task<IActionResult> Workers(Guid id)
        {
            var worker = _mapper.Map<WorkerDTO>(await _workerRepository.GetAsync(id));
            return new JsonResult(worker);
        }

        [HttpGet]
        public async Task<IActionResult> Teams()
        {
            var teams = _mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAllAsync());

          
            foreach (var team in teams)
            {
                var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
                foreach (var workerID in workers)
                {
                    var worker = await _workerRepository.GetAsync(workerID.WorkerID);
                    team.Workers.Add(worker);
                }
            }

            return new JsonResult(teams);
        }

        [HttpGet("Comparison/Teams/{id}")]
        public async Task<IActionResult> Teams(Guid id)
        {
            var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(id));
            var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
            foreach (var workerID in workers)
            {
                var worker = await _workerRepository.GetAsync(workerID.WorkerID);
                team.Workers.Add(worker);
            }

            return new JsonResult(team);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = _mapper.Map<IEnumerable<ComparisonOrderDTO>>(await _orderRepository.GetAllValidatedAsync()).ToList();
            
            for (int i = 0; i <orders.Count(); i++)
            {
                orders[i] = await MakeUpAnOrder(orders[i]);
            }

            return new JsonResult(orders);
        }

      
        [HttpGet("Comparison/Orders/{id}")] 
        public async Task<IActionResult> Orders(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            var fullOrder = _mapper.Map<ComparisonOrderDTO>(order);

            fullOrder = await MakeUpAnOrder(fullOrder);
            
            return new JsonResult(fullOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Payments()
        {
            var payment = await _paymentRepository.GetAllAsync();
            return new JsonResult(payment);
        }

        [HttpGet("Comparison/Payments/{id}")]
        public async Task<IActionResult> Payments(Guid id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return new JsonResult(payment);
        }

        private async Task<ComparisonOrderDTO> MakeUpAnOrder(ComparisonOrderDTO order)
        {
            var payment = (await _paymentRepository.GetAllAsync()).Where(x => x.OrderID == order.OrderID).SingleOrDefault();
            order.Payment = payment;
            var teams = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();

            foreach (var teamID in teams)
            {
                var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(teamID.TeamID));
                var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
              
                foreach (var workerID in workers)
                {
                    var worker = await _workerRepository.GetAsync(workerID.WorkerID);
                    team.Workers.Add(worker);
                }

                order.Teams.Add(team);
            }
            return order;
        }
    }
}