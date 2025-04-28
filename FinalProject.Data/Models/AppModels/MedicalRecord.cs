namespace FinalProject.Data.Models.AppModels
{
     public class MedicalRecord
    {
        public int Id { get; set; }

        public DateTime date { get; set; }
        public string Notes { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public int NurseId { set; get; }
        public Nurse Nurse { set; get; }

    }
}
