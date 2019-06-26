using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
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
            try
            {
                await _commandDispatcher.DispatchAsync(new EditWorker() {Worker=workerDTO});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Team([FromBody]TeamDTO teamDTO)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new EditTeam() { Team = teamDTO});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody]AdminOrderDTO adminOrderDTO)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new EditOrder() {Order= adminOrderDTO });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

    }
}