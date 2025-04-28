namespace FinalProject.Data.Models.AppModels
{
    public class Perscribtion
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
    }
}
