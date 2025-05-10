using FinalProject.Core.Feature.Cart.Requests;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
using FinalProject.Services.Implemetations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ICartServices _cartServices;
        private readonly IDoctorServices _doctorServices;
        private readonly UserManager<ApplicationUser> _userManager
;

        public CartController(ICartServices cartServices,
            IDoctorServices  doctorServices,
            UserManager<ApplicationUser> userManager
)
        {
            this._cartServices = cartServices;
            this._doctorServices = doctorServices;
            this._userManager = userManager;
        
        }
        public IActionResult Index()
        {
            var AllAppointes = _cartServices.GetAll();
            return View(AllAppointes.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var Drs = _doctorServices.GetAll();
           
            ViewBag.Drs = Drs;
 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CartRequest cartRequest)
        {
            //var userId = _userManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                var Drs = _doctorServices.GetAll();
                ViewBag.Drs = Drs;
                return View(cartRequest);
            }

            _cartServices.AddCart(cartRequest);
          
            return RedirectToAction("Index");
        }

    }
}
