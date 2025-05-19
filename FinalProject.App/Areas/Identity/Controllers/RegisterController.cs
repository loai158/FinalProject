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
        private readonly INurseServices _nurseServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPatientServices _patientServices;
        private readonly IApplicatioUserServices _applicatioUserServices;
        private readonly IDoctorServices _doctorServices;
        private readonly IDepartmentServices _departmentServices;

        public RegisterController(
            IPatientServices patientServices,
            IApplicatioUserServices applicatioUserServices,
            IDoctorServices doctorServices,
            IDepartmentServices departmentServices,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            INurseServices nurseServices,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._nurseServices = nurseServices;
            _roleManager = roleManager;
            this._patientServices = patientServices;
            this._applicatioUserServices = applicatioUserServices;
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

            // إضافة الأقسام إلى ViewBag لتمريرها إلى الـ View
            ViewBag.Departments = _departmentServices.getAll()
              .Select(d => new SelectListItem
              {
                  Value = d.Id.ToString(),
                  Text = d.Name
              }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterVM model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _departmentServices.getAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Already Exist.");
                ViewBag.Departments = _departmentServices.getAll()
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = d.Name
               }).ToList();
                return View(model);
            }
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
                model.Image = fileName;
            }

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
                ViewBag.Departments = _departmentServices.getAll()
             .Select(d => new SelectListItem
             {
                 Value = d.Id.ToString(),
                 Text = d.Name
             }).ToList();
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

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Already Exist.");
                return View(model);
            }
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


            // إنشاء مستخدم جديد
            var user = new ApplicationUser
            {
                UserName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FullName = model.FullName,
                ImgProfile = model.Image// نُخزّن الاسم في AspNetUsers أيضًا
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // إضافة الدور "Patient" للمستخدم
                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));
                }
                await _userManager.AddToRoleAsync(user, "Patient");   


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
        [HttpGet]
        public async Task<IActionResult> NurseRegister()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                await _roleManager.CreateAsync(new IdentityRole("Nurse"));
            }

            ViewBag.Departments = _departmentServices.getAll()
             .Select(d => new SelectListItem
             {
                 Value = d.Id.ToString(),
                 Text = d.Name
             }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NurseRegister(NurseRgisterVM model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _departmentServices.getAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();
                return View(model);

            }


            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ViewBag.Departments = _departmentServices.getAll()
                         .Select(d => new SelectListItem
                         {
                             Value = d.Id.ToString(),
                             Text = d.Name
                         }).ToList();
                ModelState.AddModelError("Email", "Already Exist.");
                return View(model);
            }
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
                model.Image = fileName;
            }
            // إنشاء المستخدم في AspNetUsers
            var user = new ApplicationUser
            {
                UserName = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
                ImgProfile = model.Image,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                ViewBag.Departments = _departmentServices.getAll()
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();
                return View(model);
            }


            if (!await _roleManager.RoleExistsAsync("Nurse"))
                await _roleManager.CreateAsync(new IdentityRole("Nurse"));

            await _userManager.AddToRoleAsync(user, "Nurse");
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
                model.Image = fileName;
            }
            // إنشاء السجل في جدول Doctor وربطه بالمستخدم
            var nurse = new Nurse
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Image = model.Image,
                DepartmentId = model.DepartmentId,
                IdentityUserId = user.Id
            };

            await _nurseServices.Create(nurse);
            await _signInManager.SignInAsync(user, model.RememberMe);
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}