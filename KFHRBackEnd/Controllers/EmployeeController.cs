using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFHRBackEnd.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
