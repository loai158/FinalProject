namespace FinalProject.Data.Models.AppModels
{
    public class Interaction
    {
        public int Id { get; set; }
        public string Details { get; set; }

        public Severity Severity { get; set; }
         public int? PrescribtionId {  get; set; }
        public Perscribtion? Perscribtion { get; set; }

        public int? PreviousId { get; set; }
        public PreviousMedicine? PreviousMedicine{ get; set; }

    }

    public enum Severity
    {
        Danger,
        Weak,
        Warning
    }
}
