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
    [Authorize(Roles = "Administrator")]
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
            var workers = await _workerRepository.GetAllAsync();
            return new JsonResult(workers);
        }

        [HttpGet("Comparison/Workers/{id}")]
        public async Task<IActionResult> Workers(Guid id)
        {
            var worker = await _workerRepository.GetAsync(id);
            return new JsonResult(worker);
        }

        [HttpGet]
        public async Task<IActionResult> Teams()
        {
            var team = await _teamRepository.GetAllAsync();
            return new JsonResult(team);
        }

        [HttpGet("Comparison/Teams/{id}")]
        public async Task<IActionResult> Teams(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);
            return new JsonResult(team);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var order = await _orderRepository.GetAllValidatedAsync();
            return new JsonResult(order);
        }

        [HttpGet("Comparison/Orders/{id}")]
        public async Task<IActionResult> Orders(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return new JsonResult(order);
        }

        [HttpGet("Comparison/FullOrders/{id}")] 
        public async Task<IActionResult> FullOrders(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            var fullOrder = _mapper.Map<ComparisonOrderDTO>(order);
            var payment = (await _paymentRepository.GetAllAsync()).Where(x=> x.OrderID == fullOrder.OrderID).SingleOrDefault();
            fullOrder.Payment = payment;
            fullOrder.Teams = new List<TeamDTO>();
            var teams = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == id).ToList();

            foreach (var teamID in teams)
            {
                var team = _mapper.Map<TeamDTO >(await _teamRepository.GetAsync(teamID.TeamID));
                var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
                team.Workers = new List<Worker>();

                foreach (var workerID in workers)
                {
                    var worker = await _workerRepository.GetAsync(workerID.WorkerID);
                    team.Workers.Add(worker);
                }

                fullOrder.Teams.Add(team);
            }

           
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
    }
}