using FinalProject.Data.Models.AppModels;

namespace FinalProject.Data.Models.Medical
{
    public class MedicalHistoryViM
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public ICollection<PreviousCondition> PreviousConditions { get; set; } = new List<PreviousCondition>();
        public ICollection<PreviousMedicine> PreviousMedicines { get; set; } = new List<PreviousMedicine>();
        public Name? NewConditionName { get; set; } // لإضافة حالة جديدة
        public string NewMedicineName { get; set; } // لإضافة دواء جديد
        public string NewMedicineDose { get; set; }
        public DateTime? NewMedicineStartDate { get; set; }
        public DateTime? NewMedicineEndDate { get; set; }
        // لحقول التعديل
        public int? EditConditionId { get; set; }
        public Name? EditConditionName { get; set; }
        public int? EditMedicineId { get; set; }
        public string EditMedicineName { get; set; }
        public string EditMedicineDose { get; set; }
        public DateTime? EditMedicineStartDate { get; set; }
        public DateTime? EditMedicineEndDate { get; set; }
    }
}
