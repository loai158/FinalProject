using FinalProject.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.App.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM, IFormFile? Img)
        {

            if (ModelState.IsValid)
            {
                string fileName = null;

                if (Img != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ProfImg", fileName);
                    //copy img in the wwwroot
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        Img.CopyTo(stream);
                    }
                    //save img path in db
                    registerVM.ImgProfile = fileName;
                }

                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    Address = registerVM.Address,
                    ImgProfile = registerVM.ImgProfile,
                    PhoneNumber = registerVM.PhoneNumber,

                };
                var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "Doctor");

                    TempData["Success"] = "Register Successfully!";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }



            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginVM.Email);
                if (appUser != null)
                {
                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        TempData["Error"] = "عذرًا، تم حظر حسابك ولا يمكنك تسجيل الدخول!";
                        return View(loginVM);
                    }

                    var result = await _userManager.CheckPasswordAsync(appUser, loginVM.Password);

                    if (result)
                    {
                        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
                        TempData["Success"] = "تم تسجيل الدخول بنجاح!";

                        if (await _userManager.IsInRoleAsync(appUser, "Doctor"))
                        {

                            return RedirectToAction("Home", "Doctor", new { area = "Customer" });
                        }

                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "كلمة المرور غير صحيحة!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "البريد الإلكتروني غير صحيح!");
                }
            }

            return View(loginVM);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
