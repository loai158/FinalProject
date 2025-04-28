namespace FinalProject.Data.Models.AppModels
{
    public class Nurse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; } = new HashSet<MedicalRecord>();
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
