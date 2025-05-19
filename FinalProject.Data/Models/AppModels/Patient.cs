using FinalProject.Data.Models.IdentityModels;

namespace FinalProject.Data.Models.AppModels
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ?Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string ?Address { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string IdentityUserId { get; set; }
        public ApplicationUser ?ApplicationUser { get; set; }
        public ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<PreviousCondition>? PreviousConditions { get; set; } = new HashSet<PreviousCondition>();
        public ICollection<PreviousMedicine>? PreviousMedicine { get; set; } = new HashSet<PreviousMedicine>();
    }
}
