using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.App.Utility.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IDoctorServices _doctorServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(IMessageService messageService, IDoctorServices doctorServices, UserManager<ApplicationUser> userManager)
        {
            _messageService = messageService;
            this._doctorServices = doctorServices;
            _userManager = userManager;
        }

        public async Task SendToDoctor(int doctorUserId, string message)
        {
            var senderId = _userManager.GetUserId(Context.User);
            var senderName = Context.User?.Identity?.Name ?? "مجهول";
            var id = _doctorServices.GetAll()
                .FirstOrDefault(d => d.Id == doctorUserId)?.IdentityUserId;
            await _messageService.SaveMessageAsync(senderId, id, message);

            await Clients.User(id).SendAsync("ReceiveMessage", senderName, message);
            await Clients.Caller.SendAsync("ReceiveMessage", senderName, message);
        }

        public async Task SendToPatient(string patientUserId, string message)
        {
            var senderName = Context.User?.Identity?.Name ?? "طبيب";
            var senderId = _userManager.GetUserId(Context.User);

            await _messageService.SaveMessageAsync(senderId, patientUserId, message);

            await Clients.User(patientUserId).SendAsync("ReceiveMessage", senderName, message);
            await Clients.Caller.SendAsync("ReceiveMessage", senderName, message);
        }

        // يمكنك إضافة أكواد هنا لتعقب اتصال المستخدمين أو فصلهم إذا احتجت لذلك
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // هنا ممكن تشيل المستخدم من الجروبات اللي كان فيها
            await base.OnDisconnectedAsync(exception);
        }
    }
}