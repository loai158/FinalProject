namespace FinalProject.Data.Models.AppModels
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public string Phone { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

        public Gender Gender { get; set; }
        public ICollection<Appointment>? Appointments{ get; set; } = new HashSet<Appointment>();
        public ICollection<DoctorSchedule>? DoctorSchedules{ get; set; } = new HashSet<DoctorSchedule>();

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }

}
