using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Patient.Query.Response
{
    public class GetPatientByIdResponse
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
        public List<PreviousCondition> PreviousConditions { get; set; } = new List<PreviousCondition>();
        public List<PreviousMedicine> PreviousMedicine { get; set; } = new List<PreviousMedicine>();
    }
}
