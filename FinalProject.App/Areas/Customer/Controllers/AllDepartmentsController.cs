using Azure;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AllDepartmentsController : Controller
    {
        private readonly IDepartmentServices _departmentServices;
        private readonly IDoctorServices _doctorServices;

        public AllDepartmentsController(IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {
            _departmentServices = departmentServices;
            _doctorServices = doctorServices;
        }
        public IActionResult Index(string query, string department, int page = 1)
        {
            var depts = _departmentServices.getAll().ToList();

            // filter by name
            if (!string.IsNullOrEmpty(query))
            {
                depts = depts.Where(e => e.Name.Contains(query)).ToList();
            }

            // pagination
            var paginationPages = (int)Math.Ceiling((decimal)depts.Count() / 7);
            if (page < 1) page = 1;
            if (page > paginationPages && paginationPages > 0) page = paginationPages;

            depts = depts.Skip((page - 1) * 7).Take(7).ToList();

            ViewBag.paginationPages = paginationPages;
            ViewBag.CurrentPage = page;
            ViewBag.Query = query;

            return View(depts);
        }
        public IActionResult DocsDepartment(string dep)
        {
            // Get department by name
            var department = _departmentServices.getAll().FirstOrDefault(d => d.Name == dep);
            if (department == null)
            {
                return NotFound("Department not found");
            }

            // Get doctors by department id
            var doctors = _doctorServices.GetByDeptId(department.Id).ToList();

            return View(doctors);
        }
    }
}
