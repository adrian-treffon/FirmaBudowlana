using System.Threading.Tasks;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.Commands.Team;
using FirmaBudowlana.Infrastructure.Commands.Worker;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EditController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public EditController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {
           
           await _commandDispatcher.DispatchAsync(new EditWorker() {Worker=workerDTO});

           return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody]TeamDTO teamDTO)
        {
           
           await _commandDispatcher.DispatchAsync(new EditTeam() { Team = teamDTO});

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody]AdminOrderDTO adminOrderDTO)
        {
           
           await _commandDispatcher.DispatchAsync(new EditOrder() {Order= adminOrderDTO });

           return Ok();
        }

    }
}