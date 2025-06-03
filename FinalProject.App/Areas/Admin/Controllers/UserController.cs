using FinalProject.App.Helper.EmailSettings;
using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.SendEmailModel;
using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserRepository _applicationUser;
        private readonly IDoctorRepositry _doctorRepositry;
        private readonly IPatientRepositry _patientRepositry;
        private readonly INurseRepositry _nurseRepositry;
        private readonly IEmailSettings _emailSettings;
        private readonly IDepartmentServices _departmentServices;

        public UserController(UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork,
            IApplicationUserRepository applicationUser, IDoctorRepositry doctorRepositry,
            IPatientRepositry patientRepositry, INurseRepositry nurseRepositry,
            IEmailSettings emailSettings, IDepartmentServices departmentServices
            )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._unitOfWork = unitOfWork;
            this._applicationUser = applicationUser;
            this._doctorRepositry = doctorRepositry;
            this._patientRepositry = patientRepositry;
            this._nurseRepositry = nurseRepositry;
            this._emailSettings = emailSettings;
            this._departmentServices = departmentServices;
        }
        [HttpGet]
        public IActionResult Index(string query, int page = 1)
        {
            var users = _applicationUser.Get();
            //filter
            if (query != null)
            {
                users = users.Where(e => e.UserName.Contains(query)
                || e.Email.Contains(query));
            }
            //pagination
            var paginationPages = (int)Math.Ceiling((decimal)users.Count() / 7);
            if (page < 1) page = 1;
            if (page > paginationPages && paginationPages > 0) page = paginationPages;
            users = users.Skip((page - 1) * 7).Take(7);
            ViewBag.paginationPages = paginationPages;
            return View(users.ToList());

        }
        [HttpGet]
        public async Task<IActionResult> SendEmail(string id)
        {
            var user = await _applicationUser.GetOne(e => e.Id == id); // جرب تدور بـ Id مباشرًا الأول

            if (user == null)
            {
                TempData["Error"] = "not found";
                return RedirectToAction("Index");
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult SendEmail(Email email)
        {
            if (email != null)
            {
                var mail = new Email()
                {
                    To = email.To,
                    Subject = email.Subject,
                    Body = email.Body
                };
                _emailSettings.SendEmail(mail);
                return RedirectToAction("Index");
            }

            return View();
        }



        [HttpGet]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.roles = roles;
            var deps = _departmentServices.getAll();
            ViewBag.deps = deps;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientRegisterVM registerVM, string RoleType, int departmentId)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = registerVM.FullName,
                    Email = registerVM.Email,
                    Address = registerVM.Address,
                    PhoneNumber = registerVM.Phone
                };

                var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, RoleType);

                    // check role and adding doctor if true
                    if (string.Equals(RoleType?.Trim(), "Doctor", StringComparison.OrdinalIgnoreCase))
                    {
                        Doctor doctor = new Doctor
                        {
                            Name = registerVM.FullName,
                            Email = registerVM.Email,
                            Phone = registerVM.Phone,
                            Gender = registerVM.Gender,
                            IdentityUserId = applicationUser.Id,
                            ApplicationUser = applicationUser,
                            Image = registerVM.Image,
                            DepartmentId = departmentId,
                            IntialPrice = 0,
                            FollowUpPrice = 0,
                            Details = "Default details"
                        };

                        await _doctorRepositry.Create(doctor);
                        await _unitOfWork.CompleteAsync();
                    }

                    // check role and adding patient if true
                    else if (string.Equals(RoleType?.Trim(), "Patient", StringComparison.OrdinalIgnoreCase))
                    {
                        Patient patient = new Patient
                        {
                            Name = registerVM.FullName,
                            Email = registerVM.Email,
                            Phone = registerVM.Phone,
                            Gender = registerVM.Gender,
                            IdentityUserId = applicationUser.Id,
                            ApplicationUser = applicationUser,
                            Image = registerVM.Image,
                            Address = registerVM.Address,
                            DateOfBirth = registerVM.DateOfBirth
                        };

                        await _patientRepositry.Create(patient);
                        await _unitOfWork.CompleteAsync();
                    }

                    // check role and adding nurse if true
                    else if (string.Equals(RoleType?.Trim(), "Nurse", StringComparison.OrdinalIgnoreCase))
                    {
                        Nurse nurse = new Nurse
                        {
                            Name = registerVM.FullName,
                            Email = registerVM.Email,
                            Phone = registerVM.Phone,
                            IdentityUserId = applicationUser.Id,
                            ApplicationUser = applicationUser,
                            Image = registerVM.Image,
                            DepartmentId = departmentId
                        };

                        await _nurseRepositry.Create(nurse);
                        await _unitOfWork.CompleteAsync();
                    }


                    TempData["Success"] = "Account Created Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            var roles = _roleManager.Roles.ToList();
            ViewBag.roles = roles;
            return View();
        }



        public async Task<IActionResult> Delete(string userId)
        {
            var userAcount = await _applicationUser.GetOne(e => e.Id == userId);
            if (userAcount != null)
            {
                _applicationUser.Delete(userAcount);
                _unitOfWork.ApplicationUserRepository.Commit();
                TempData["Success"] = "Account Deleted Successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
