using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AddController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public AddController(IMapper mapper, IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            _mapper = mapper;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {
           if(workerDTO == null) return BadRequest(new { message = "ERROR" });

           var worker =_mapper.Map<Worker>(workerDTO);
           worker.WorkerID = Guid.NewGuid();
           await _workerRepository.AddAsync(worker);
           return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Team()
        {
            var workers = await _workerRepository.GetAllAsync();
            var team = new TeamDTO()
            {
                Workers = workers,
                Description = ""
            };

            return new JsonResult(team);
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody] TeamDTO teamDTO)
        {

            if (teamDTO == null) return BadRequest(new { message = "ERROR" });

            var team = _mapper.Map<Team>(teamDTO);
            team.WorkersTeams = new List<WorkerTeam>();
            team.TeamID = Guid.NewGuid();

            foreach (var worker in teamDTO.Workers)
            {
                team.WorkersTeams.Add(
                    new WorkerTeam()
                    {
                        Team = team,
                        Worker = worker,
                        TeamID= team.TeamID,
                        WorkerID = worker.WorkerID
                    }
              );
            }

            await _teamRepository.AddAsync(team);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromBody]OrderToPaidDTO orderToPaidDTO)
        {
            if (orderToPaidDTO == null) return BadRequest(new { message = "ERROR" });

            var payment = _mapper.Map<Payment>(orderToPaidDTO);
            payment.PaymentDate = DateTime.UtcNow;
            var order = await _orderRepository.GetAsync(orderToPaidDTO.OrderID);

            foreach (var team in orderToPaidDTO.Teams)
            {
                var workersID = team.WorkersTeams.Where(x => x.TeamID == team.TeamID).Select(x => x.WorkerID);

                foreach (var workerID in workersID)
                {
                    var worker = await _workerRepository.GetAsync(workerID);
                    var days = payment.PaymentDate.DayOfYear - order.StartDate.DayOfYear;
                    payment.PaymentID = Guid.NewGuid();
                    payment.Amount = worker.ManHour * 8 * days;
                    await _paymentRepository.AddAsync(payment);
                }
            }

            order.Paid = true;
            await _orderRepository.UpdateAsync(order);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            IEnumerable<Order> unPaidOrders = await _orderRepository.GetAllUnpaidAsync();
            
            var ordersToPaidDTO = _mapper.Map<IEnumerable<OrderToPaidDTO>>(unPaidOrders);

            foreach (var unPaidOrderDTO in ordersToPaidDTO)
            {
                foreach (var order in unPaidOrders)
                {
                    unPaidOrderDTO.Teams.Append(await _teamRepository.GetAsync(order.OrdersTeams.Where(x => x.OrderID == order.OrderID).Select(x => x.TeamID).SingleOrDefault()));
                }
                
            }

            return new JsonResult(ordersToPaidDTO);
        }
    }
}