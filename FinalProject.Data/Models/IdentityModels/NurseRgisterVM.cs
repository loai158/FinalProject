using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class NurseRgisterVM
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Image { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
