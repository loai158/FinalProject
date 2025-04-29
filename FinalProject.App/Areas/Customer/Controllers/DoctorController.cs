using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Feature.Doctor.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;

        public DoctorController( IMediator mediator )
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery());
            return View(response);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(AddDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            if(doctorId == -1)
            {
                TempData["ErrorMessage"] = " الاسم موجود  بالفعل";
                return View();
            }
             

                TempData["SuccessMessage"] = "تم إضافة الطبيب بنجاح";
                return RedirectToAction("Index");
             

           
        }

    }
}
