using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.VM;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(IDepartmentServices departmentServices)
        {
            this._departmentServices = departmentServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string query = "")
        {
            var departmentsQuery = _departmentServices.getAll();

            if (!string.IsNullOrWhiteSpace(query))
            {
                departmentsQuery = departmentsQuery.Where(d =>
                    d.Name.Contains(query) || d.Location.Contains(query) || d.Description.Contains(query));
            }

            var totalItems = await departmentsQuery.CountAsync();
            var departments = await departmentsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new DepartmentListViewModel
            {
                Departments = departments,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchQuery = query
            };


            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentServices.AddDepartmentAsync(model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while adding the department: " + ex.Message;
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int deptId)
        {
            var department = await _departmentServices.GetByIdAsync(deptId);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentServices.UpdateAsync(model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while updating the department: " + ex.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int deptId)
        {
            var department = await _departmentServices.GetByIdAsync(deptId);

            if (department == null)
            {
                TempData["Error"] = "Department not found.";
                return RedirectToAction("Index");
            }

            await _departmentServices.DeleteAsync(deptId);

            TempData["Success"] = "Department deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
