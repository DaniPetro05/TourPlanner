using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TourPlanner;

namespace TourPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> GetRequest()
        {
            
        }*/
    }
}
