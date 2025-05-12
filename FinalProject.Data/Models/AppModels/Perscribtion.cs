namespace FinalProject.Data.Models.AppModels
{
    public class Perscribtion
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<Medicine> Medicines { get; set; } = new HashSet<Medicine>();
    }
}
