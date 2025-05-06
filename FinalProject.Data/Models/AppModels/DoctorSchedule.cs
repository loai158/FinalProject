namespace FinalProject.Data.Models.AppModels
{
    public class DoctorSchedule
    {
        public int Id { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

    }
}
