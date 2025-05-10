using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Patient.Query.Response
{
    public class GetAllPatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<PreviousCondition>? PreviousConditions { get; set; }
        public ICollection<PreviousMedicine>? PreviousMedicine { get; set; }

    }

}
