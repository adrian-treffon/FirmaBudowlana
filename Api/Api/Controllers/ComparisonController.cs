using System;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public ComparisonController(IMapper mapper, IOrderRepository orderRepository, IWorkerRepository workerRepository, 
            ITeamRepository teamRepository, IPaymentRepository paymentRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _paymentRepository = paymentRepository;
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