namespace FinalProject.Data.Models.AppModels
{
    public class Perscribtion
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<PerscribtionMedicine> PerscribtionMedicines { get; set; }
    }
}
