using FinalProject.Data.Models.AppModels;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.Medical
{
    public class PrescriptionVM
    {
        public int? AppointmentId { get; set; }

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public ICollection<PerscribtionMedicine>? PerscribtionMedicines { get; set; }

        public int? NewMedicineId { get; set; }

        [Display(Name = "اسم الدواء")]
        public string? NewMedicineName { get; set; }

        [Display(Name = "الجرعة")]
        public int? NewMedicineDose { get; set; }

        [Display(Name = "تاريخ البدء")]
        public DateTime? NewMedicineStartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? NewMedicineEndDate { get; set; }

        public int? EditMedicineId { get; set; }
        public string? EditMedicineName { get; set; }
        public string? EditMedicineDose { get; set; }
        public DateTime? EditMedicineStartDate { get; set; }
        public DateTime? EditMedicineEndDate { get; set; }
    }
}
