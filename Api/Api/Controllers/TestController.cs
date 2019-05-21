using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FirmaBudowlana.Api.Controllers
{
    public class TestController : Controller
    {
        // GET: /<controller>/
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
