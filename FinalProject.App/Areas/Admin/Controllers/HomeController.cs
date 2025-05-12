using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }
    }
}
