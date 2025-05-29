namespace FinalProject.Data.Models.Medical
{
    public class PatientMessagesViewModel
    {
        public string DoctorName { get; set; }
        public int DoctorId { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new();
    }

}
