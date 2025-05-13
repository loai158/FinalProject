using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;

namespace FinalProject.Data.Models.PaymentModels
{
    public class Cart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DoctorSchedule? DoctorSchedule { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

    }
}


