using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientServices _patientServices;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IPatientServices patientServices, IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _patientServices = patientServices;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginVM.Email);
                if (appUser != null)
                {
                    // check block
                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        TempData["Error"] = "Sorry, your account is blocked!";
                        return View(loginVM);
                    }

                    var result = await _userManager.CheckPasswordAsync(appUser, loginVM.Password);
                    if (result)
                    {
                        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
                        TempData["Success"] = "Login Successful!";

                        // جلب الدور
                        var roles = await _userManager.GetRolesAsync(appUser);
                        if (roles.Contains("Doctor"))
                        {
                            return RedirectToAction("Profile", "Doctor", new { area = "Customer" });
                        }

                        if (roles.Contains("Patient"))
                        {
                            return RedirectToAction("Profile", "Patient", new { area = "Customer" });
                        }

                        if (roles.Contains("Nurse"))
                        {
                            return RedirectToAction("Profile", "Nurse", new { area = "Customer" });
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid password. Try again!");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid email. Try again!");
                }
            }
            return View(loginVM);
        }

        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("Login", "Account");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            // تحقق إذا كان المستخدم موجود
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // أنشئ مستخدم جديد
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    // رجّع خطأ لو حصلت مشكلة في إنشاء المستخدم
                    return RedirectToAction("Login", "Account");
                }
                await _userManager.AddToRoleAsync(user, "Patient");
                var patient = new Patient
                {
                    IdentityUserId = user.Id,
                    Name = name,
                    Email = email,
                    Gender = Gender.Male
                };
                await _patientServices.Create(patient);

                // ربط الدخول الخارجي بالمستخدم
                await _userManager.AddLoginAsync(user, new UserLoginInfo(
                    result.Principal.Identity.AuthenticationType,
                    result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    result.Principal.Identity.AuthenticationType));
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);

              return RedirectToAction("Profile", "Patient", new { area = "Customer" });
        }

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    if (!result.Succeeded)
        //        return RedirectToAction("Login", "Account");

        //    var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        //    var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

        //    // تحقق إذا كان المستخدم موجود
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        // new user
        //        user = new ApplicationUser
        //        {
        //            UserName = email,
        //            Email = email
        //        };

        //        var createResult = await _userManager.CreateAsync(user);
        //        if (!createResult.Succeeded)
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }

        //        // adding him as Patient
        //        await _userManager.AddToRoleAsync(user, "Patient");

        //        // ربط الدخول الخارجي بالمستخدم
        //        await _userManager.AddLoginAsync(user, new UserLoginInfo(
        //            result.Principal.Identity.AuthenticationType,
        //            result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        //            result.Principal.Identity.AuthenticationType));

        //        //  add  Patient 
        //        var patient = new Patient
        //        {
        //            IdentityUserId = user.Id,
        //            Name = name,
        //            Email = email,
        //            Gender = Gender.Male
        //        };
        //        _patientServices.Create(patient);


        //    }

        //    // تسجيل الدخول
        //    await _signInManager.SignInAsync(user, isPersistent: false);

        //    return RedirectToAction("Profile", "Patient", new { area = "Customer" });
        //}



        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });

        }
    }
}
