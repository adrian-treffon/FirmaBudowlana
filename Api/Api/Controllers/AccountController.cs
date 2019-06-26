using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.User;
using FirmaBudowlana.Infrastructure.Services;
using Komis.Infrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirmaBudowlana.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDispatcher _commandDispatcher;

        public AccountController(IUserService userService, IOrderRepository orderRepository, 
            IMapper mapper, ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _orderRepository = orderRepository;
            _mapper= mapper;
            _commandDispatcher= commandDispatcher;
         
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO userParam)
        {
            var command = new Login() { LoginCredentials = userParam };
            try
            {
                await _commandDispatcher.DispatchAsync(command);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
           
            return new JsonResult(command.Token);
        }

        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO userParam)
        {
            if (userParam == null) return BadRequest(new { message = $"Post request account/register is empty" });

            await _userService.Register(userParam.FirstName, userParam.LastName, userParam.Address, userParam.Email, userParam.Password);

            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> UserOrders(Guid id)
        {
            var accesToken = Request.Headers["Authorization"];

            try
            {
                if (Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id) return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Incorrect token" });
            }
           
            var orders = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == id);

            var ordersDTO = _mapper.Map<IEnumerable<AdminOrderDTO>>(orders);

            return new JsonResult(ordersDTO);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Account/OrderDetails/{idUser}/{idOrder}")]
        public async Task<IActionResult> OrderDetails(Guid idUser, Guid idOrder)
        {
            var accesToken = Request.Headers["Authorization"];

            try
            {
                if (Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != idUser) return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Incorrect token" });
            }

            var order = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == idUser).SingleOrDefault(x => x.OrderID == idOrder);

            if (order == null)
                return BadRequest(new { message = $"Cannot find order {idOrder} in DB" });

            var dto = _mapper.Map<AdminOrderDTO>(order);
            
            return new JsonResult(dto);
        }
         


    }
}
