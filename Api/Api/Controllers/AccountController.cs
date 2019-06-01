using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _userService = userService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper= mapper;
         
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO userParam)
        {
            var token = await _userService.Login(userParam.Email, userParam.Password);

            var user = await _userRepository.GetAsync(userParam.Email);

            if (token == null && user == null)
                return BadRequest(new { message = " or password is incorrect" });

            var dto = new TokenDTO()
            {
                Token = token,
                User = user
            };

            return new JsonResult(dto);
        }

        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO userParam)
        {
            await _userService.Register(userParam.FirstName, userParam.LastName, userParam.Address, userParam.Email, userParam.Password);

            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Orders([FromBody]Guid userID)
        {
            var result = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == userID);

            if (result == null)
                return BadRequest(new { message = "Incorrect id" });

            var dto = _mapper.Map<IEnumerable<AdminOrderDTO>>(result);

            return new JsonResult(dto);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Orders([FromBody]Guid userID, [FromBody]Guid orderid)
        {
            var result = (await _orderRepository.GetAllAsync()).Where(x => x.UserID == userID).Single(x => x.OrderID == orderid);

            if (result == null)
                return BadRequest(new { message = "Incorrect id" });

            var dto = _mapper.Map<AdminOrderDTO>(result);
             // dto.Teams = null;
                
            return new JsonResult(dto);
        }
         


    }
}
