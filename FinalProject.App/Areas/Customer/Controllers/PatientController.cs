using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Models;
using FinalProject.Data.Models.IdentityModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PatientController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            this._mediator = mediator;
            this._userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllPatientQuery());
            return View(response);


        }
        [HttpGet]
        public IActionResult Create([FromQuery] int doctorId)
        {
            var model = new AddNewPatient
            {
                DoctorId = doctorId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewPatient command, IFormFile? file)
        {
            var userId = _userManager.GetUserId(User);
            command.IdentityUserId = userId;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Patients", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                command.Image = fileName;
            }
            try
            {
                var PatientId = await _mediator.Send(command);

                if (PatientId == -1)
                {
                    TempData["Error"] = "الاسم موجود بالفعل";
                    return View(command);
                }

                TempData["Success"] = "تم إضافة المريض بنجاح";

                return RedirectToAction("Create", "Cart", new { area = "Customer", doctorId = command.DoctorId, patientId = PatientId });

            }
            catch (FluentValidation.ValidationException ex)
            {
                // أضف الأخطاء إلى ModelState
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(command); // هذا يعرض الأخطاء في الـ View
            }
        }

    }
}
