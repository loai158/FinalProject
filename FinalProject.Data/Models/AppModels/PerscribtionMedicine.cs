namespace FinalProject.Data.Models.AppModels
{
    public class PerscribtionMedicine
    {
        public int Id { get; set; } // مفتاح أساسي

        public int PerscribtionId { get; set; }
        public Perscribtion Perscribtion { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        // ممكن تضيف خصائص إضافية هنا، زي مثلاً الجرعة أو ملاحظات
        public int Dose { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
