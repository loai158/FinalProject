using FinalProject.Core.Feature.Apponitments.Command.Models;
using FinalProject.Core.Feature.Apponitments.Query.Models;
using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientServices _patientServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IDoctorServices _doctorServices;

        public AppointmentController(IMediator mediator, IUnitOfWork unitOfWork, IPatientServices patientServices, IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {
            this._departmentServices = departmentServices;
            this._doctorServices = doctorServices;
            this._mediator = mediator;
            this._unitOfWork = unitOfWork;
            this._patientServices = patientServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var respone = await _mediator.Send(new GetAllApponintmentsQuery
            {
                Query = query,
                Page = page,
                PageSize = 10
            });
            return View(respone);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Departments"] = _departmentServices.getAll().ToList();
            ViewData["Doctors"] = _doctorServices.GetAll().ToList();
            ViewData["Patients"] = _patientServices.GetAll().ToList();
            return View();
        }
        [HttpGet]
        public JsonResult GetDoctorsByDepartment(int departmentId)
        {
            var doctors = _doctorServices.GetAll()
                           .Where(d => d.DepartmentId == departmentId)
                           .Select(d => new { d.Id, d.Name })
                           .ToList();
            return Json(doctors);
        }
        [HttpGet]
        public IActionResult GetAvailableDatesByDoctor(int doctorId)
        {
            var schedules = _unitOfWork.Repositry<DoctorSchedule>().Get()
                .Where(s => s.DoctorId == doctorId && s.IsAvailable)
                .Select(s => new
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .Distinct()
                .ToList();

            var today = DateTime.Today;
            var futureDates = new List<object>();

            for (int i = 0; i < 14; i++) // 14 يوم قدام
            {
                var date = today.AddDays(i);
                var matchingSchedule = schedules.FirstOrDefault(s => s.Day == date.DayOfWeek);

                if (matchingSchedule != null)
                {
                    futureDates.Add(new
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        StartTime = matchingSchedule.StartTime.ToString(@"hh\:mm"),// بصيغة 08:30 مثلاً
                        EndTime = matchingSchedule.EndTime.ToString(@"hh\:mm") // بصيغة 08:30 مثلاً
                    });
                }
            }

            return Json(futureDates);
        }
        [HttpGet]
        public async Task<IActionResult> GetPriceByStatus(int doctorId, int status)
        {
            var doctor = await _doctorServices.GetById(doctorId);
            if (doctor == null)
                return NotFound();

            decimal price = 0;

            if ((Status)status == Status.Initial)
                price = (decimal)doctor.IntialPrice;
            else if ((Status)status == Status.FollowUp)
                price = (decimal)doctor.FollowUpPrice;

            return Json(price);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewAppointmentCommand command)
        {
            var appointmentId = await _mediator.Send(command);
            if (appointmentId == -1)
            {
                TempData["Error"] = " هذا الحجز موجود بالفعل ";
                return View();
            }
            TempData["Success"] = "تم إضافة الحجز بنجاح";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int appointmentId)
        {
            var query = new GetAppointmentByIdQuery(appointmentId);
            var response = await _mediator.Send(query);
            if (response != null)
            {
                ViewData["Departments"] = _departmentServices.getAll().ToList();
                ViewData["Doctors"] = _doctorServices.GetAll().ToList();
                ViewData["Patients"] = _patientServices.GetAll().ToList();
                return View(response);
            }
            return RedirectToAction("Error", "Home", new { area = "Customer" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAppointmentCommand model)
        {

            var response = await _mediator.Send(model);

            if (response)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Departments = _departmentServices.getAll()
           .Select(d => new SelectListItem
           {
               Value = d.Id.ToString(),
               Text = d.Name
           })
            .ToList();
            return View(model);
        }
        public async Task<IActionResult> Delete(int appointmentId)
        {
            var response = await _mediator.Send(new DeleteAppointmentCommand(appointmentId));
            if (response == "success")
            {
                TempData["Success"] = "تم الحذف بنجاح";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Error"] = "يرجي حذف الريكورات اولا";
                return RedirectToAction("index");
            }
        }
    }
}
