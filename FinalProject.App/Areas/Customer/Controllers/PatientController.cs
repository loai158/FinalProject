using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PatientController : Controller
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllPatientQuery());
            return View(response);

             
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewPatient command)
        {
            try
            {
                var patientId = await _mediator.Send(command);
                if (patientId == -1)
                {
                    TempData["ErrorMessage"] = "الاسم موجود بالفعل";
                    return View(command);
                }
                else if (patientId == 0)
                {
                    TempData["ErrorMessage2"] = "حدث خطأ أثناء التسجيل";
                    return View(command);
                }

                TempData["SuccessMessage"] = "تم إضافة المريض بنجاح";
                return RedirectToAction("Index");
            }
            catch (ValidationException ex)
            {
                // إضافة أخطاء الـ Validation للـ ModelState
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(command);
            }
        }
    }
}
