using FinalProject.Core.Feature.Cart.Requests;
using FinalProject.Services.Abstracts;
using FinalProject.Services.Implemetations;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ICartServices _cartServices;
        private readonly IDoctorServices _doctorServices;

        public CartController(ICartServices cartServices,
            IDoctorServices  doctorServices
)
        {
            this._cartServices = cartServices;
            this._doctorServices = doctorServices;
        
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
