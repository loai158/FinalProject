using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;

        private readonly IDoctorServices _doctorServices;
        private readonly IPatientServices _patientServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IDoctorServices doctorServices, IPatientServices patientServices, IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _messageService = messageService;
            this._doctorServices = doctorServices;
            this._patientServices = patientServices;
        }
        public async Task<ActionResult> Index(int doctorId)
        {
            // جلب اسم الدكتور
            var doctor = await _doctorServices.GetById(doctorId);
            ViewBag.DoctorName = doctor?.Name;
            ViewBag.DoctorId = doctorId;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesWithDoctor(int doctorId)
        {
            var currentUserId = _userManager.GetUserId(User);

            var doctor = _doctorServices.GetAll().FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
                return NotFound("الطبيب غير موجود");

            var doctorUserId = doctor.IdentityUserId;

            var messages = await _messageService.GetMessagesAsync(currentUserId, doctorUserId);

            var result = messages.Select(m => new MessageViewModel
            {
                SenderName = m.Sender.UserName,
                Content = m.Content,
                SentAt = m.SentAt
            }).ToList();

            return Json(result);
        }
        public async Task<IActionResult> AllMessages()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Doctor"))
            {
                var doctor = await _doctorServices.GetAll().Where(d => d.IdentityUserId == userId).FirstOrDefaultAsync();

                var messages = await _messageService.GetPatientsWithMessages(doctor.Id);
                var viewModel = new DoctorMessagesViewModel
                {
                    DoctorId = doctor.Id,
                    Patients = messages
                };
                return View("DoctorAllMessages", viewModel);
            }
            else if (roles.Contains("Patient"))
            {
                var patient = await _patientServices.GetAll().Where(d => d.IdentityUserId == userId).FirstOrDefaultAsync();

                var messages = await _messageService.GetDoctorsWithMessagesForPatient(patient.Id);
                var viewModel = new PatientMessagesViewModel
                {
                    PatientId = patient.Id,
                    Doctors = messages
                };
                return View("PatientAllMessages", viewModel);
            }

            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesWithPatient(string patientId)
        {
            var doctorUserId = _userManager.GetUserId(User);
            var messages = await _messageService.GetMessageWithPatient(doctorUserId, patientId);

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessageAsync(string receiverId, string content)
        {
            var senderId = _userManager.GetUserId(User);
            await _messageService.SaveMessageAsync(senderId, receiverId, content);
            return Json(new { success = true });

        }
    }
}
