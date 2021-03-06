﻿using System;
using System.Threading.Tasks;
using FirmaBudowlana.Infrastructure.Commands.Order;
using FirmaBudowlana.Infrastructure.Commands.Team;
using FirmaBudowlana.Infrastructure.Commands.Worker;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirmaBudowlana.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeleteController : Controller
    {

        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;

        }

        [HttpGet]
        public async Task<IActionResult> Worker(Guid id)
        {
            
            await _commandDispatcher.DispatchAsync(new DeleteWorker() { WorkerID = id });
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Team(Guid id)
        {
           
            await _commandDispatcher.DispatchAsync(new DeleteTeam() { TeamID = id });
          
            return Ok();

        }

        [HttpGet]
        public async Task<IActionResult> Order(Guid id)
        {
           
            await _commandDispatcher.DispatchAsync(new DeleteOrder() { OrderID = id });

            return Ok();

        }
    }
}