namespace FinalProject.Data.Models.VM
{
    public class DoctorMessageVM
    {
        public string DoctorId { get; set; }           // IdentityUserId
        public string DoctorName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
