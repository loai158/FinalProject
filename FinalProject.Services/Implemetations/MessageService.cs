using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Data.Models.VM;
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

            var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();

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

                    return new PatientChatPreviewViewModel
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

        public async Task<List<PatientChatPreviewViewModel>> GetPatientsWithMessages(int id)
        {
            var doctor = await _unitOfWork.Repositry<Doctor>().GetOne(d => d.Id == id);

            if (doctor == null) return new List<PatientChatPreviewViewModel>();

            var doctorUserId = doctor.IdentityUserId;

            var messages = await _unitOfWork.Repositry<Message>().Get()
                .Where(m => m.ReceiverId == doctorUserId || m.SenderId == doctorUserId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            // تحديد المرضى المتكلمين مع الدكتور
            var patientMessages = messages
                .Where(m => m.SenderId != doctorUserId || m.ReceiverId != doctorUserId)
                .GroupBy(m => m.SenderId == doctorUserId ? m.ReceiverId : m.SenderId)
                .Select(g =>
                {
                    var lastMsg = g.First();
                    var patientUser = lastMsg.SenderId == doctorUserId ? lastMsg.Receiver : lastMsg.Sender;

                    return new PatientChatPreviewViewModel
                    {
                        PatientId = patientUser.Id,
                        PatientName = patientUser.UserName,
                        LastMessage = lastMsg.Content,
                        LastMessageTime = lastMsg.SentAt
                    };
                })
                .OrderByDescending(p => p.LastMessageTime)
                .ToList();

            return patientMessages;
        }

        public async Task<List<DoctorMessageVM>> GetDoctorsWithMessagesForPatient(int patientId)
        {
            var patient = await _unitOfWork.Repositry<Data.Models.AppModels.Patient>()
                                          .GetOne(p => p.Id == patientId);
            if (patient == null)
                return new List<DoctorMessageVM>();

            var patientUserId = patient.IdentityUserId;

            var messages = await _unitOfWork.Repositry<Message>()
                                            .Get()
                                            .Where(m => m.SenderId == patientUserId || m.ReceiverId == patientUserId)
                                            .OrderByDescending(m => m.SentAt)
                                            .ToListAsync();

            // احصل على قائمة الـ DoctorUserIds المميزة
            var doctorUserIds = messages
                .Select(m => m.SenderId == patientUserId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToList();

            // استعلم أسماء الأطباء مرة واحدة فقط
            var doctors = await _unitOfWork.Repositry<ApplicationUser>()
                                           .Get()
                                           .Where(u => doctorUserIds.Contains(u.Id))
                                           .Select(u => new { u.Id, u.UserName })
                                           .ToListAsync();

            var doctorMessages = messages
                .Select(m => new
                {
                    DoctorUserId = m.SenderId == patientUserId ? m.ReceiverId : m.SenderId,
                    Message = m
                })
                .GroupBy(x => x.DoctorUserId)
                .Select(g =>
                {
                    var doctor = doctors.FirstOrDefault(d => d.Id == g.Key);
                    return new DoctorMessageVM
                    {
                        DoctorId = g.Key,
                        DoctorName = doctor?.UserName ?? "Unknown",
                        LastMessage = g.First().Message.Content,
                        LastMessageTime = g.First().Message.SentAt
                    };
                })
                .ToList();

            return doctorMessages;
        }


    }

}
