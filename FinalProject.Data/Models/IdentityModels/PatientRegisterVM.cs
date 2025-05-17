using FinalProject.Data.Models.AppModels;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{

    public class PatientRegisterVM
    {
        [Required(ErrorMessage = "الرجاء إدخال الاسم الكامل")]

        public string FullName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]

        public string Email { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال كلمة المرور")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الهاتف")]
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]

        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال العنوان")]

        public string Address { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار الجنس")]

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال تاريخ الميلاد")]
        [DataType(DataType.Date)]

        public DateTime DateOfBirth { get; set; }
        public string? Image { get; set; }
        public bool RememberMe { get; set; }
    }
}
