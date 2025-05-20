namespace FinalProject.Data.Models.AppModels
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public int Dose { get; set; }
        public ICollection<PerscribtionMedicine> PerscribtionMedicines { get; set; }

        public DateTime EndDate { get; set; }
    }
}
