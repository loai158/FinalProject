namespace FinalProject.Core.Feature.PreviousMedicines.Command.Models
{
    public class AddNewPreviousMedicine
    {
        public string Name { get; set; }
        public string Dose { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
