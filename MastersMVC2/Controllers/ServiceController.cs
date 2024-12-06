using Microsoft.AspNetCore.Mvc;

namespace MastersMVC2.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
