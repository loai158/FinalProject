using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.Medical;

namespace FinalProject.Services.Abstracts
{
    public interface IMessageService
    {
        Task SaveMessageAsync(string senderId, string receiverId, string content);
        //Task<List<Message>> GetMessagesForDoctorAsync(string doctorUserId);
        public Task<DoctorMessagesViewModel> GetMessagesForDoctorAsync(string doctorId);
        Task<List<Message>> GetMessageWithPatient(string? doctorUserId, string patientId);
        public Task<List<Message>> GetMessagesAsync(string userId1, string userId2);
    }

}
