using FinalProject.Data.Models.VM;

namespace FinalProject.Data.Models.Medical
{
    public class PatientMessagesViewModel
    {

        public int PatientId { get; set; }
        public List<DoctorMessageVM> Doctors { get; set; }
    }



}
