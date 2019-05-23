using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FirmaBudowlana.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO userParam)
        {
            var token = await _userService.Login(userParam.Email, userParam.Password);

            if (token == null)
                return BadRequest(new { message = " or password is incorrect" });

            return Content(token);
        }

      
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO userParam)
        {
            await _userService.Register(userParam.FirstName, userParam.LastName, userParam.Address, userParam.Email, userParam.Password);

            var token = await _userService.Login(userParam.Email, userParam.Password);

            if (string.IsNullOrEmpty(token.ToString()))
                return BadRequest(new { message = "Email or password is incorrect" });

            return Content(token);
        }

    }
}
