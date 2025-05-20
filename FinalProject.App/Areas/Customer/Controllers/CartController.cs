using FinalProject.Core.Feature.Apponitments.Command.Models;
using FinalProject.Core.Feature.Apponitments.Query.Models;
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
        private readonly IDepartmentServices _departmentServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientServices _patientServices;

        public CartController(IMediator mediator, UserManager<ApplicationUser> userManager, IDoctorServices doctorServices, IDepartmentServices departmentServices, IAppointmentServices appointmentServices, IPatientServices patientServices)
        {
            this._mediator = mediator;
            this._doctorServices = doctorServices;
            this._departmentServices = departmentServices;
            this._appointmentServices = appointmentServices;
            this._userManager = userManager;
            this._patientServices = patientServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var response = await _mediator.Send(new GetAllApponintmentsQuery
            {
                Query = query,
                Page = page,
                PageSize = 10,
                id = userId
            });

            ViewBag.CurrentQuery = query;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(response.TotalCount / 10.0);

            return View(response);
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

            var DeptId = await _departmentServices.findDeptByDocId(command.DoctorId);

            command.DepartmentId = DeptId;
            var response = await _mediator.Send(command);
            if (response == 0)
                return RedirectToAction("Create", "Cart", new { area = "Customer", doctorId = command.DoctorId, patientId = command.PatientId });

            return RedirectToAction("Index");
        }


    }
}
