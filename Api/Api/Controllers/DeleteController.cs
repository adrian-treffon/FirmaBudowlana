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

            await _workerRepository.RemoveAsync(worker);

            await DeleteEmptyTeam();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Team(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);

            if (team == null) return BadRequest(new { message = $"Cannot find the team {id} in DB" });

            await _teamRepository.RemoveAsync(team);

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

            foreach (var team in teams)
            {
                if (!(workerteam.Select(x => x.TeamID).Contains(team.TeamID))) await _teamRepository.RemoveAsync(team);
            }

        }
    }
}