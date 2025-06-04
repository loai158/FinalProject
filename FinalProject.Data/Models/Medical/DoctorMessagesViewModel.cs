using FinalProject.Data.Models.VM;

namespace FinalProject.Data.Models.Medical
{

    public class DoctorMessagesViewModel
    {
        public int DoctorId { get; set; }
        public List<PatientChatPreviewViewModel> Patients { get; set; }
    }


}
