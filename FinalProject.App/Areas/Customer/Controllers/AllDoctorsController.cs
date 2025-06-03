using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AllDoctorsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentServices _departmentServices;
        private readonly IDoctorServices _doctorServices;
     
        public AllDoctorsController(IMediator mediator, IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {

            _mediator = mediator;
            this._departmentServices = departmentServices;
            this._doctorServices = doctorServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string query, string department, int page = 1)
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery{});

            var deps = _departmentServices.getAll();
            ViewBag.deps = deps;
            //filter with name&email
            if (query != null)
            {
                response = response.Where(e => e.Name.Contains(query)
                || e.Email.Contains(query));
            } 
            //filter with depaerments
            if (department != null)
            {
                response = response.Where(e => e.Department == department);
            }
            //pagination
            var paginationPages = (int)Math.Ceiling((decimal)response.Count() / 7);
            if (page < 1) page = 1;
            if (page > paginationPages && paginationPages > 0) page = paginationPages;
            response = response.Skip((page - 1) * 7).Take(7);
            ViewBag.paginationPages = paginationPages;
            return View(response);
        }
    }
}
