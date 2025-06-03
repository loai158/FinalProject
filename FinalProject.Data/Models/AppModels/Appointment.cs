namespace FinalProject.Data.Models.AppModels
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int DepartmentId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public Department Department { get; set; }
        public Status Status { get; set; }
        public TypePayment TypePayment { get; set; }
        public Perscribtion? Perscribtion { get; set; }
        public int ScheduleId { get; set; }
        public DoctorSchedule Schedule { get; set; }
        public decimal Price { get; set; }

    }

    public enum TypePayment
    {
        Online,
        Cash
    }
    public enum Status
    {
        Initial,
        FollowUp
    }
}