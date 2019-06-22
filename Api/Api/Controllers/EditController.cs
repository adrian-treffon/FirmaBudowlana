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
    [Authorize(Roles ="Admin")]
    public class EditController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DBContext _context;

        public EditController(IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IOrderRepository orderRepository,IMapper mapper, DBContext context)
        {
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {
            if (workerDTO == null) return BadRequest(new { message = "Post request edit/worker is empty" });
           
            var workerFromDB = await _workerRepository.GetAsync(workerDTO.WorkerID);

            if (workerFromDB == null) return BadRequest(new { message = $"Cannot find the worker {workerDTO.WorkerID} in DB" });

            await _workerRepository.UpdateAsync(_mapper.Map<Worker>(workerDTO));

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody]TeamDTO teamDTO)
        {
            if (teamDTO == null) return BadRequest(new { message = "Post request edit/team is empty" });

            var teamFromDB = await _teamRepository.GetAsync(teamDTO.TeamID);
          
            if (teamFromDB == null) return BadRequest(new { message = $"Cannot find the team {teamDTO.TeamID} in DB" });

            var team = _mapper.Map<Team>(teamDTO);

            foreach (var worker in teamDTO.Workers)
            {
                team.WorkerTeam.Add(
                   new WorkerTeam()
                   {
                       Team = team,
                       Worker = worker,
                       TeamID = team.TeamID,
                       WorkerID = worker.WorkerID
                   }
             );
            }

            var workerTeam = (await _context.WorkerTeam.ToListAsync()).Where(x => x.TeamID == team.TeamID).ToList();
            _context.WorkerTeam.RemoveRange(workerTeam);

            await _context.WorkerTeam.AddRangeAsync(team.WorkerTeam);
            await _context.SaveChangesAsync();
            await _teamRepository.UpdateAsync(team);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody]AdminOrderDTO adminOrderDTO)
        {
            if (adminOrderDTO == null) return BadRequest(new { message = "Post request edit/order is empty" });

            var orderFromDB = await _orderRepository.GetAsync(adminOrderDTO.OrderID);

            if (orderFromDB == null) return BadRequest(new { message = $"Cannot find the order {adminOrderDTO.OrderID} in DB" });

            var order = _mapper.Map<Order>(adminOrderDTO);

            if (orderFromDB.Validated == false) return BadRequest(new { message = $"You cannot edit invalidated order" });

            if (orderFromDB.Paid == true) return BadRequest(new { message = $"You cannot edit paid/finished order" });


            foreach (var team in adminOrderDTO.Teams)
            {
                order.OrderTeam.Add(
                    new OrderTeam
                    {
                        Order = order,
                        OrderID = order.OrderID,
                        Team = team,
                        TeamID = team.TeamID
                    }
                    );
            }

            order.Validated = true;

            var orderTeam = (await _context.OrderTeam.ToListAsync()).Where(x => x.OrderID == order.OrderID).ToList();
            _context.OrderTeam.RemoveRange(orderTeam);

            await _context.OrderTeam.AddRangeAsync(order.OrderTeam);
            await _orderRepository.UpdateAsync(order);

            return Ok();
        }

    }
}