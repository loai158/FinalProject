using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;

        private readonly IDoctorServices _doctorServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IDoctorServices doctorServices, IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _messageService = messageService;
            this._doctorServices = doctorServices;
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
        public async Task<IActionResult> AllMessage()
        {
            var doctorUserId = _userManager.GetUserId(User);
            var messages = await _messageService.GetMessagesForDoctorAsync(doctorUserId);
            return View(messages);
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
