using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        => new JsonResult(_mapper.Map<IEnumerable<WorkerDTO>>(await _workerRepository.GetAllActiveAsync()));
            
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

            for (int i = 0; i < orders.Count(); i++)
            {
                orders[i] = await MakeUpAnOrder(orders[i]);
            }

            return new JsonResult(orders);
        }


        [HttpGet("Comparison/Orders/{id}")]
        public async Task<IActionResult> Orders(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order == null) return BadRequest(new { message = $"Cannot find the order {id} in DB" });

            var fullOrder = _mapper.Map<ComparisonOrderDTO>(order);

            fullOrder = await MakeUpAnOrder(fullOrder);

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

        private async Task<ComparisonOrderDTO> MakeUpAnOrder(ComparisonOrderDTO order)
        {

            var payments = (await _paymentRepository.GetAllAsync()).Where(x => x.OrderID == order.OrderID).ToList();

            if (payments.Any())
            {
                order.Payments = payments;
                order.Paid = true;
            }
            else order.Payments = null;


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

        [HttpPost]
        public async Task<IActionResult> Report([FromBody]ReportDTO report)
        {
           var orders = new List<ComparisonOrderDTO>();

            if (report.Teams.Any() || report.Workers.Any())
            {
                var ordersID = new List<Guid>();

                if (report.Teams.Any() && !report.Workers.Any())
                {
                    foreach (var team in report.Teams)
                    {
                        ordersID.AddRange(_context.OrderTeam.Where(x => x.TeamID == team.TeamID).Select(x => x.OrderID).ToList());
                    }

                }
                else if (!report.Teams.Any() && report.Workers.Any())
                {
                    var teamIDs = new List<Guid>();

                    foreach (var worker in report.Workers)
                    {
                        teamIDs.AddRange(_context.WorkerTeam.Where(x => x.WorkerID == worker.WorkerID).Select(x => x.TeamID).ToList());
                    }

                    foreach (var teamID in teamIDs)
                    {
                        ordersID.AddRange(_context.OrderTeam.Where(x => x.TeamID == teamID).Select(o => o.OrderID).ToList());
                    }

                }else if(report.Teams.Any() && report.Workers.Any()) return BadRequest(new { message = $"You can only choose teams or workers, not both" });

                foreach (var id in ordersID)
                {
                    var order = _mapper.Map<ComparisonOrderDTO>(await _orderRepository.GetAsync(id));

                    if (!orders.Select(o => o.OrderID).Contains(order.OrderID))
                    {
                        orders.Add(order);
                    }

                }
            }
            else if (!report.Teams.Any() || !report.Workers.Any())
            {
                orders= _mapper.Map<List<ComparisonOrderDTO>>(await _orderRepository.GetAllValidatedAsync());
            }

            if (report.StartDate != null && report.EndDate != null) orders = orders.Where(x => x.StartDate >= report.StartDate && x.StartDate <= report.EndDate).ToList();

            for (int i = 0; i < orders.Count(); i++)
            {
                orders[i] = await MakeUpAnOrder(orders[i]);
            }

            orders = orders.OrderBy(x => x.StartDate).ToList();

            return new JsonResult(orders);
        }
    }
}