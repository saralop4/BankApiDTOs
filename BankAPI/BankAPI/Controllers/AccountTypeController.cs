using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountTypeController : Controller
    {
        [NonAction]
        public IActionResult Index()
        {
            return View();
        }
    }
}
