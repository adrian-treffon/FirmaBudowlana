using System;
using System.Collections;
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
    [Authorize]
    public class AddController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly DBContext _context;

        public AddController(IMapper mapper, IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository, IPaymentRepository paymentRepository,
            DBContext context)
        {
            _mapper = mapper;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _context = context;
        }

        [HttpPost]
        //dodaje nowego pracownika
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {
           if(workerDTO == null) return BadRequest(new { message = "ERROR" });

           var worker =_mapper.Map<Worker>(workerDTO);
           worker.WorkerID = Guid.NewGuid();
           await _workerRepository.AddAsync(worker);
           return Ok();
        }

        [HttpGet]
        //zwraca gotowy team z wszystkimi pracownikami
        //usuwamy niepotrzebnych pracwonikow i oddajemy mi team 
        public async Task<IActionResult> Team()
        {
            var workers = (await _workerRepository.GetAllAsync()).ToList();
            var team = new TeamDTO()
            {
                Workers = workers,
                Description = "",
                TeamID = Guid.NewGuid()
            };

            return new JsonResult(team);
        }

        [HttpPost]
        //z teamDTO tworze prawdzimy team i wsadzam do bazy
        public async Task<IActionResult> Team([FromBody] TeamDTO teamDTO)
        {
            if (teamDTO == null) return BadRequest(new { message = "ERROR" });

            var team = _mapper.Map<Team>(teamDTO);
          
            foreach (var worker in teamDTO.Workers)
            {
                team.WorkerTeam.Add(
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
        //płacę za zlecenie
        public async Task<IActionResult> Payment([FromBody]OrderToPaidDTO orderToPaidDTO)
        {
            if (orderToPaidDTO == null) return BadRequest(new { message = "ERROR" });

            var payment = _mapper.Map<Payment>(orderToPaidDTO);
            payment.PaymentDate = DateTime.UtcNow;
            var order = await _orderRepository.GetAsync(orderToPaidDTO.OrderID);

            foreach (var teamID in orderToPaidDTO.Teams)
            {
                var team = await _teamRepository.GetAsync(teamID.TeamID);

                foreach (var ele in team.WorkerTeam)
                {
                    var worker = await _workerRepository.GetAsync(ele.WorkerID);
                    var days = order.EndDate.Value.DayOfYear - order.StartDate.DayOfYear;
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
        //lista wszystich niezaplaconych zlecen
        //zawiera wszytskie zespoły wraz z pracownikami i kosztem wszystkich wypłat
        public async Task<IActionResult> Payment()
        {
            var unPaidOrders = (await _orderRepository.GetAllUnpaidAsync()).ToList();
            if (unPaidOrders == null) return NotFound(new { message = "Cannot find any orders in DB" });

            var ordersDTO = _mapper.Map<IEnumerable<OrderToPaidDTO>>(unPaidOrders);

            foreach (var order in ordersDTO)
            {
                var teams = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();
                var days = order.EndDate.DayOfYear - order.StartDate.DayOfYear;

                var startDate = order.StartDate;

                
                while (startDate.Date != order.EndDate.Date)
                {
                    if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday) days--;
                    startDate = startDate.AddDays(1);
                }


                foreach (var teamID in teams)
                {
                    var team = _mapper.Map<TeamDTO>(await _teamRepository.GetAsync(teamID.TeamID));
                    var workers = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();

                    foreach (var workerID in workers)
                    {
                        var worker = await _workerRepository.GetAsync(workerID.WorkerID);
                        team.Workers.Add(worker);
                        order.PaymentCost += worker.ManHour * 8 * days;
                    }

                    order.Teams.Add(team);
                }
              
            }

            return new JsonResult(ordersDTO);
        }

    }
}