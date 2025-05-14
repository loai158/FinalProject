using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Models;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
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
        private readonly IAppointmentServices _appointmentServices;
        private readonly IPatientServices _patientServices;

        public PatientController(IMediator mediator, UserManager<ApplicationUser> userManager, IAppointmentServices appointmentServices, IPatientServices patientServices)
        {
            this._mediator = mediator;
            this._userManager = userManager;
            this._appointmentServices = appointmentServices;
            this._patientServices = patientServices;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllPatientQuery());
            return View(response);


        }
        [HttpGet]
        public IActionResult Create([FromQuery] int doctorId)
        {
            var patients = _patientServices.GetAll().Select(e => new { e.IdentityUserId, e.Id });

            var userId = _userManager.GetUserId(User);
            foreach (var patient in patients)
            {
                if (patient.IdentityUserId == userId)
                {
                    TempData["Error"] = "الاسم موجود بالفعل";
                    return RedirectToAction("Create", "Cart", new { area = "Customer", doctorId = doctorId, patientId = patient.Id });
                }
                else
                {
                    var model = new AddNewPatient
                    {
                        DoctorId = doctorId
                    };
                    return View(model);
                }

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewPatient command, IFormFile? file)
        {

            var userId = _userManager.GetUserId(User);
            command.IdentityUserId = userId;

            var PatientId = await _mediator.Send(command);



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
