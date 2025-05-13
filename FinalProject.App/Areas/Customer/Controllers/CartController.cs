using FinalProject.Core.Feature.Apponitments.Command.Models;
using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDoctorServices _doctorServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientServices _patientServices;
        private readonly FinalProject.Core.CustomeServices.Cart.ICartServices _cartServices;

        public CartController(
            IMediator mediator,
            IDoctorServices doctorServices,
            IAppointmentServices appointmentServices,
            UserManager<ApplicationUser> userManager,
            IPatientServices patientServices,
            FinalProject.Core.CustomeServices.Cart.ICartServices cartServices)
        {
            this._mediator = mediator;
            this._doctorServices = doctorServices;
            this._appointmentServices = appointmentServices;
            this._userManager = userManager;
            this._patientServices = patientServices;
            this._cartServices = cartServices;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int doctorId, int patientId)
        {
            var Dr = await _doctorServices.GetById(doctorId);
            ViewBag.Dr = Dr;
            var patient = await _patientServices.GetById(patientId);
            ViewBag.Patient = patient;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddNewAppointmentCommand command)
        {
            var userId = _userManager.GetUserId(User);
            var id = await _appointmentServices.GetPatientIdFromUserAsync(userId);
            if (id == null)
            {
                ModelState.AddModelError("", "يرجى إكمال بيانات المريض أولاً.");
                return View(command);
            }
            command.PatientId = (int)id;
            Doctor deptId = _doctorServices.GetByDeptId(command.DoctorId).FirstOrDefault();

            command.DoctorId = deptId.Id;
            var response = await _mediator.Send(command);

            if (!ModelState.IsValid)
                return View(command);
            return RedirectToAction("Index");
        }

    }
}
