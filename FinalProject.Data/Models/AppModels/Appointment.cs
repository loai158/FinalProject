namespace FinalProject.Data.Models.AppModels
{
    public class Appointment
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int DepartmentId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public Department Department { get; set; }
        public Status Status { get; set; }
        public ICollection<Perscribtion>? Perscribtions { get; set; } = new HashSet<Perscribtion>();


    }
    public enum Status
    {
        Initial,
        FollowUp
    }
}