using System;
using System.Linq;
using System.Threading.Tasks;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeleteController : Controller
    {

        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;

        public DeleteController(IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository,DBContext context)
        {
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Worker(Guid id)
        {
            var worker = await _workerRepository.GetAsync(id);

            if (worker == null) return BadRequest(new { message = $"Cannot find the worker {id} in DB" });

            var teamIDs = _context.WorkerTeam.Where(x => x.WorkerID == id).Select(y => y.TeamID);

            foreach (var teamID in teamIDs)
            {
                var ordersID =_context.OrderTeam.Where(x => x.TeamID == teamID).Select(y => y.OrderID);

                foreach (var orderID in ordersID)
                {
                    var order = await _orderRepository.GetAsync(orderID);
                    if(!order.Paid) return BadRequest(new { message = $"Cannot deactivate the worker {id}, because of active orders" });
                }

            }

            worker.Active = false;

            await _workerRepository.UpdateAsync(worker);

            await DeleteEmptyTeam();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Team(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);

            if (team == null) return BadRequest(new { message = $"Cannot find the team {id} in DB" });

            var orderTeam = _context.OrderTeam.Where(x => x.TeamID == team.TeamID).Select(o => o.OrderID).ToList();

            foreach (var orderID in orderTeam)
            {
                var order = await _orderRepository.GetAsync(orderID);
                if(!order.Paid) return BadRequest(new { message = $"Cannot deactivate the team {team.TeamID}, because of active orders" });
            }

            team.Active = false;

            await _teamRepository.UpdateAsync(team);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Order(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order == null) return BadRequest(new { message = $"Cannot find the order {id} in DB" });

            if(order.Validated == true) return BadRequest(new { message = $"You cannot remove validated order" });

            await _orderRepository.RemoveAsync(order);

            return Ok();
        }


        private async Task DeleteEmptyTeam()
        {
            var workerteam = await _context.WorkerTeam.ToListAsync();
            var teams = await _teamRepository.GetAllAsync();
            var workers = await _workerRepository.GetAllAsync();

            foreach (var team in teams)
            {
                var workersInTeam = workerteam.Where(x => x.TeamID == team.TeamID).Select(c => c.WorkerID).ToList();
                int inactiveWorkers = 0;
                foreach (var workerID in workersInTeam)
                {
                    var worker = workers.Where(x => x.WorkerID == workerID).SingleOrDefault();
                    if (worker.Active == false) inactiveWorkers++;
                }
                if (inactiveWorkers == workersInTeam.Count() || workersInTeam.Count() == 0) await Team(team.TeamID);
            }

        }
    }
}