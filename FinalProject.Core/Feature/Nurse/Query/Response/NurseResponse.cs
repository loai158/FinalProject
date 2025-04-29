using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Nurse.Query.Response
{
    public class NurseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; } 

        public string Department { get; set; }
    }
}
