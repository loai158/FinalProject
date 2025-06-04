using Azure;
using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Data.Models;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly IDepartmentServices _departmentServices;

        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IDoctorServices _doctorServices;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {
            _mediator = mediator;
            this._departmentServices = departmentServices;
            this._doctorServices = doctorServices;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var allDocs = await _mediator.Send(new GetAllDoctorsQuery());
            var pagedDocs = allDocs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var allDepts = _departmentServices.getAll();
            var pagedDepts = allDepts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Departments"] = pagedDepts;
            return View(pagedDocs);
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
