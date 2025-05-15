using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.App.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPatientServices _patientServices;
        private readonly IDoctorServices _doctorServices;
        private readonly IDepartmentServices _departmentServices;

        public RegisterController(
            IPatientServices patientServices,
            IDoctorServices doctorServices,
            IDepartmentServices departmentServices,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this._patientServices = patientServices;
            _doctorServices = doctorServices;
            this._departmentServices = departmentServices;
        }

        [HttpGet]
        public async Task<IActionResult> DoctorRegister()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                await _roleManager.CreateAsync(new IdentityRole("Nurse"));
            }

            var departments = _departmentServices.getAll().ToList();
            // إضافة الأقسام إلى ViewBag لتمريرها إلى الـ View
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // إنشاء المستخدم في AspNetUsers
            var user = new ApplicationUser
            {
                UserName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                ImgProfile = model.Image
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // إضافة Role Doctor لو مش موجود
            if (!await _roleManager.RoleExistsAsync("Doctor"))
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));

            await _userManager.AddToRoleAsync(user, "Doctor");

            // إنشاء السجل في جدول Doctor وربطه بالمستخدم
            var doctor = new Doctor
            {
                Name = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Image = model.Image,
                Details = model.Details,
                IntialPrice = model.IntialPrice,
                FollowUpPrice = model.FollowUpPrice,
                Gender = model.Gender,
                DepartmentId = model.DepartmentId,
                IdentityUserId = user.Id
            };

            await _doctorServices.Create(doctor);
            await _signInManager.SignInAsync(user, model.RememberMe);
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
        [HttpGet]
        public async Task<IActionResult> PatientRegister()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                await _roleManager.CreateAsync(new IdentityRole("Nurse"));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientRegister(PatientRegisterVM model, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return View(model);

            // إنشاء مستخدم جديد
            var user = new ApplicationUser
            {
                UserName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FullName = model.FullName  // نُخزّن الاسم في AspNetUsers أيضًا
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // إضافة الدور "Patient" للمستخدم
                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));
                }
                await _userManager.AddToRoleAsync(user, "Patient");  // إضافة المستخدم للدور:contentReference[oaicite:1]{index=1}

                // إنشاء كيان المريض المرتبط بالمستخدم
                var patient = new Patient
                {
                    IdentityUserId = user.Id,
                    Name = model.FullName,
                    Phone = model.Phone,
                    Address = model.Address,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    Image = model.Image

                };
                if (file != null && file.Length > 0)
                {
                    // Save img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Patients", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save img name in db
                    model.Image = fileName;
                }


                await _patientServices.Create(patient);

                // تسجيل دخول المستخدم (مع خيار التذكر)
                await _signInManager.SignInAsync(user, model.RememberMe);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // إضافة الأخطاء إن وجدت
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}