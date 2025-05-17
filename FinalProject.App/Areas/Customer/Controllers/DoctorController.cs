using FinalProject.Core.Feature.Apponitments.Query.Models;
using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Data.Models.AppModels;
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

    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentServices _departmentServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IDoctorServices _doctorServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientServices _patientServices;

        public DoctorController(IMediator mediator, IDepartmentServices departmentServices, IAppointmentServices appointmentServices, IDoctorServices doctorServices, UserManager<ApplicationUser> userManager, IPatientServices patientServices)
        {
            _mediator = mediator;
            this._departmentServices = departmentServices;
            this._appointmentServices = appointmentServices;
            this._doctorServices = doctorServices;
            this._userManager = userManager;
            this._patientServices = patientServices;
        }
        [HttpGet]
        public IActionResult MoreDetails(int deptId)
        {
            var response = _doctorServices.GetByDeptId(deptId).ToList();

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> Home(string? query, int page = 1)
        {
            var userId = _userManager.GetUserId(User);
            Doctor doctor = _doctorServices.GetAll().FirstOrDefault(d => d.IdentityUserId == userId);
            //  var id = await _appointmentServices.GetPatientIdFromUserAsync(userId);
            var response = await _mediator.Send(new GetAllApponintmentsByDoctorIdQuery
            {
                doctorId = doctor.Id,
                Query = query,
                Page = page,
                PageSize = 10
            });
            ViewBag.CurrentQuery = query;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(response.TotalCount / 10.0);
            return View(response);
        }













        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var query = new GetDoctorByIdQuery(id);
        //    var response = await _mediator.Send(query);

        //    if (response == null)
        //    {
        //        TempData["ErrorMessage"] = "الدكتور غير موجود";
        //        return NotFound();
        //    }

        //    // تحويل GetDoctorByIdResponse إلى EditDoctorCommand
        //    var command = new EditDoctorCommand
        //    {
        //        Id = response.Id,
        //        Name = response.Name,
        //        //Departments = response.Departments,
        //        // DepatrmentId = response.DepartmentId,
        //        Phone = response.Phone,
        //        Email = response.Email,
        //        Image = response.Image,
        //        DoctorSchedules = response.DoctorSchedules,
        //        Gender = response.Gender,
        //        Details = response.Details,

        //        // ضيف باقي الخصائص هنا حسب الموديل عندك
        //    };

        //    return View(command); // View بتستقبل EditDoctorCommand
        //}
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            if (doctorId == -1)
            {
                TempData["ErrorMessage"] = " الاسم موجود  بالفعل";
                return View();
            }

            TempData["SuccessMessage"] = "تم إضافة الطبيب بنجاح";
            return RedirectToAction("Index");

        }

        // عرض البروفايل
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var doctor = await _doctorServices.GetAll()
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id);

            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var doctor = await _doctorServices.GetAll().FirstOrDefaultAsync(d => d.Id == id);


            if (doctor == null) return NotFound();

            var vm = new DoctorEditVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Phone = doctor.Phone,
                Image = doctor.Image,
                Details = doctor.Details,
                IntialPrice = doctor.IntialPrice,
                FollowUpPrice = doctor.FollowUpPrice,
                Gender = doctor.Gender,
                DepartmentId = doctor.DepartmentId,
                Email = doctor.Email,
            };
            var departments = _departmentServices.getAll().ToList();
            // إضافة الأقسام إلى ViewBag لتمريرها إلى الـ View
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorEditVM vm, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Doctors", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save img name in db
                vm.Image = fileName;
            }
            if (!ModelState.IsValid) return View(vm);

            var doctor = await _doctorServices.GetAll().FirstOrDefaultAsync(d => d.Id == vm.Id);

            if (doctor == null) return NotFound();

            doctor.Name = vm.Name;
            doctor.Phone = vm.Phone;
            doctor.Image = vm.Image;
            doctor.Details = vm.Details;
            doctor.IntialPrice = vm.IntialPrice;
            doctor.FollowUpPrice = vm.FollowUpPrice;
            doctor.Gender = vm.Gender;
            doctor.DepartmentId = vm.DepartmentId;

            _doctorServices.Edit(doctor);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

    }
}
