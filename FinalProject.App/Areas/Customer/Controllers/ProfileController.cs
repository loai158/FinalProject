using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.IRepositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ProfileController(IApplicationUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);  

            if (user != null)
            {
                var userData = new RegisterVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    ImgProfile = user.ImgProfile
                };
                return View(userData);
            }

            return View(new RegisterVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterVM registerVM, IFormFile? Img, string OldPassword)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                // التحقق من كلمة المرور القديمة إذا تم تقديمها
                if (!string.IsNullOrEmpty(registerVM.Password))
                {
                    if (string.IsNullOrEmpty(OldPassword))
                    {
                        TempData["Error"] = "Enter OldPasssword to Change Data!";
                        return View(registerVM);
                    }

                    // التحقق من تطابق كلمة المرور القديمة
                    var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, OldPassword);
                    if (!isOldPasswordCorrect)
                    {
                         TempData["Error"] = "OldPassword is Not true!";
                        return View(registerVM);
                    }
                }

                string fileName = null;

                if (Img != null)
                {
                    // حذف الصورة القديمة إذا كانت موجودة
                    if (!string.IsNullOrEmpty(user.ImgProfile))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ProfImg", user.ImgProfile);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ProfImg", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await Img.CopyToAsync(stream);
                    }

                    registerVM.ImgProfile = fileName;
                    user.ImgProfile = fileName;
                }

                user.UserName = registerVM.UserName;
                user.Email = registerVM.Email;
                user.Address = registerVM.Address;
                user.PhoneNumber = registerVM.PhoneNumber;

                // تغيير كلمة المرور إذا تم تقديم كلمة مرور جديدة
                if (!string.IsNullOrEmpty(registerVM.Password) && !string.IsNullOrEmpty(OldPassword))
                {
                    var changePassword = await _userManager.ChangePasswordAsync(user, OldPassword, registerVM.Password);

                    if (!changePassword.Succeeded)
                    {
                        foreach (var item in changePassword.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View(registerVM);
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["Success"] = "Data Updated Successfully!";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(registerVM);
        }
     

    }
}
