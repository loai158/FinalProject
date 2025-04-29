using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.IRepositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Edit()
        {
            var userId = _userManager.GetUserId(User);
            var user = _userRepository.GetOne(e => e.Id == userId);

            //if (user != null)
            //{
            //    var userData = new RegisterVM
            //    {
            //        UserName = user.UserName,
            //        Email = user.Email,
            //        Address = user.Address,
            //        PhoneNumber = user.PhoneNumber,
            //        ImgProfile = user.ImgProfile
            //    };
            //    return View(userData);
            //}
            return View(new RegisterVM());
        }
       
    }
}
