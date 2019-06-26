using System;
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
    [Authorize(Roles = "Admin")]
    public class AddController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AddController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Worker([FromBody]WorkerDTO workerDTO)
        {

            try
            {
                await _commandDispatcher.DispatchAsync(new AddWorker() {Worker= workerDTO });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody] TeamDTO teamDTO)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new AddTeam() { Team = teamDTO});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromBody]OrderToPaidDTO orderToPaidDTO)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new PayOrder() { Order = orderToPaidDTO});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var command = new FillOrdersToPaid();
            try
            {
                await _commandDispatcher.DispatchAsync(command);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return new JsonResult(command.Orders);
        }

    }
}