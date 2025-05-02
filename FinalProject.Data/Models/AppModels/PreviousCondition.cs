namespace FinalProject.Data.Models.AppModels
{
    public class PreviousCondition
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
    public enum Name
    {
        Diabetes,
        Hypertension,
        Heart,
        Arthritis,
        Asthma
    }
}
