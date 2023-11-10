using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionTypeController : Controller
    {
        [NonAction]
        public IActionResult Index()
        {
            return View();
        }
    }
}
