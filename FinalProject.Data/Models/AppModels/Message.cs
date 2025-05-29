using FinalProject.Data.Models.IdentityModels;

namespace FinalProject.Data.Models.AppModels
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }       // ممكن يكون مريض
        public string ReceiverId { get; set; }     // الدكتور
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;

        // علاقات اختيارية
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }

}
