using FinalProject.Data.Models;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IDepartmentServices _departmentServices;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IDepartmentServices departmentServices)
        {
            this._departmentServices = departmentServices;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var depts = _departmentServices.getAll().ToList();
            ViewData["Departments"] = depts;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
