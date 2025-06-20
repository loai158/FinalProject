using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
