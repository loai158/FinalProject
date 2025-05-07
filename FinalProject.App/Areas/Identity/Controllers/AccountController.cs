using FinalProject.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

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
                await _roleManager.CreateAsync(new IdentityRole("Company"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

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
                    PhoneNumber = registerVM.PhoneNumber,
                    ImgProfile = fileName
                };
                var result =await _userManager.CreateAsync(applicationUser, registerVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "Admin");
                    TempData["Success"] = "Register Successfully!";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                else
                {
                    ModelState.AddModelError("Password", "Not Match the constrains");
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
                    //check block
                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        TempData["Error"] = "Sorry your Account Is Blocking you Con Not Use It !";
                        return View(loginVM);
                    }
                    var result = await _userManager.CheckPasswordAsync(appUser, loginVM.Password);

                    if (result)
                    {
                        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
                        TempData["Success"] = "Login Successfully !";
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid Password Try Again!");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid Email Try Again!");
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
