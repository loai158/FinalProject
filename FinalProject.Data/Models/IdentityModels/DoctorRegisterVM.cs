using FinalProject.Data.Models.AppModels;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class DoctorRegisterVM
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Phone { get; set; }

        public string? Image { get; set; }

        [Required]
        public string Details { get; set; }

        public decimal? IntialPrice { get; set; }
        public decimal? FollowUpPrice { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
