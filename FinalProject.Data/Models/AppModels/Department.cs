namespace FinalProject.Data.Models.AppModels
{
    public class Department
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }

        public ICollection<Doctor>? Doctors{ get; set; } = new HashSet<Doctor>();
        public ICollection<Appointment>? Appointments{ get; set; } = new HashSet<Appointment>();
        public ICollection<Nurse>? Nurses{ get; set; } = new HashSet<Nurse>();
    }
}
