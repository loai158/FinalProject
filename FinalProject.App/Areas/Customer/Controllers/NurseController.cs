using FinalProject.Core.Feature.Nurse.Query.Models;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NurseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDepartmentServices _departmentServices;
        private readonly INurseServices _nurseServices;

        public NurseController(IMediator mediator, UserManager<ApplicationUser> userManager, IDepartmentServices departmentServices, INurseServices nurseServices)
        {
            _mediator = mediator;
            this._userManager = userManager;
            this._departmentServices = departmentServices;
            this._nurseServices = nurseServices;
        }

        public async Task<IActionResult> GetById([FromQuery] GetNurseByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }


        public IActionResult Index()
        {
            return View();
        }
        ////////////
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var nurse = _nurseServices.GetAll().Include(e => e.Department).Where(e => e.IdentityUserId == user.Id).FirstOrDefault();

            if (nurse == null) return NotFound();

            return View(nurse);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var nurse = await _nurseServices.GetAll().Include(e => e.Department).FirstOrDefaultAsync(d => d.Id == id);
            if (nurse == null) return NotFound();

            var vm = new NurseEditVM
            {
                Id = nurse.Id,
                Name = nurse.Name,
                Phone = nurse.Phone,
                Email = nurse.Email,
                Image = nurse.Image,
                DepartmentId = nurse.DepartmentId,
            };
            var departments = _departmentServices.getAll().ToList();
            // إضافة الأقسام إلى ViewBag لتمريرها إلى الـ View
            ViewBag.Departments = new SelectList(departments, "Id", "Name");


            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NurseEditVM vm, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Nurses", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save img name in db
                vm.Image = fileName;
            }
            if (!ModelState.IsValid) return View(vm);

            var nurse = await _nurseServices.GetAll().FirstOrDefaultAsync(d => d.Id == vm.Id);

            if (nurse == null) return NotFound();

            nurse.Name = vm.Name;
            nurse.Phone = vm.Phone;
            nurse.Image = vm.Image;
            nurse.DepartmentId = vm.DepartmentId;
            nurse.Email = vm.Email;

            _nurseServices.Edit(nurse);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }










    }
}
