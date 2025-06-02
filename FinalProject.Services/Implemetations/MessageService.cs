using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services.Implemetations
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<List<Message>> GetMessagesAsync(string userId1, string userId2)
        {
            return await _unitOfWork.Repositry<Message>().Get()
                .Include(m => m.Sender)
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        public async Task SaveMessageAsync(string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.Now
            };

            var result = await _unitOfWork.Repositry<Message>().Create(message);
            if (result == "success")
                await _unitOfWork.CompleteAsync();

        }
        public async Task<DoctorMessagesViewModel> GetMessagesForDoctorAsync(string doctorUserId)
        {
            var messages = await _unitOfWork.Repositry<Message>()
                   .Get(m => m.ReceiverId == doctorUserId)
                   .ToListAsync();

            // استخرج كل الـ SenderId بشكل مميز
            var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();

            // اجلب كل المرضى المرتبطين بالـ SenderIds
            var patients = await _unitOfWork.Repositry<Data.Models.AppModels.Patient>()
                .Get(p => senderIds.Contains(p.IdentityUserId))
                .ToListAsync();

            // جمّع البيانات
            var patientVMs = messages
                .GroupBy(m => m.SenderId)
                .Select(g =>
                {
                    var patient = patients.FirstOrDefault(p => p.IdentityUserId == g.Key);
                    var lastMessage = g.OrderByDescending(m => m.SentAt).FirstOrDefault();

                    return new PatientMessageVM
                    {
                        PatientId = g.Key,
                        PatientName = patient?.Name ?? "مجهول",
                        LastMessage = lastMessage?.Content,
                        LastMessageTime = (DateTime)(lastMessage?.SentAt)
                    };
                })
                .ToList();

            return new DoctorMessagesViewModel
            {
                Patients = patientVMs
            };
        }

        public async Task<List<Message>> GetMessageWithPatient(string? doctorUserId, string patientId)
        {
            var messages = await _unitOfWork.Repositry<Message>()
                   .Get()
                   .Where(m => (m.SenderId == patientId && m.ReceiverId == doctorUserId) ||
                               (m.SenderId == doctorUserId && m.ReceiverId == patientId))
                   .OrderBy(m => m.SentAt)
                   .Select(m => new Message
                   {
                       SenderId = m.SenderId,
                       Content = m.Content,
                       SentAt = m.SentAt
                   })
                   .ToListAsync();
            return messages;
        }


    }

}
