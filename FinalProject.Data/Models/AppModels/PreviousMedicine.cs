namespace FinalProject.Data.Models.AppModels
{
    public class PreviousMedicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
