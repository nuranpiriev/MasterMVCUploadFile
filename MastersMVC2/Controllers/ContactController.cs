using Microsoft.AspNetCore.Mvc;

namespace MastersMVC2.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
