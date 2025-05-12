namespace FinalProject.Data.Models.AppModels
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Dose { get; set; }
        public ICollection<Perscribtion> Perscribtions { set; get; } = new HashSet<Perscribtion>();
    }
}
