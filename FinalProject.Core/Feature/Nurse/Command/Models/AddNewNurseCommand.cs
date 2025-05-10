using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Core.Feature.Nurse.Command.Models
{
    public class AddNewNurseCommand : IRequest<int>
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يزيد عن 100 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?\d{5,15}$", ErrorMessage = "رقم الهاتف غير صالح، يجب أن يكون بين 10 و15 رقم")]
        public string Phone { get; set; }

        public string? Image { get; set; } // اختياري، مش محتاج تحقق

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب اختيار قسم")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار قسم صالح")]
        public int DepatrmentId { get; set; }
    }
}
