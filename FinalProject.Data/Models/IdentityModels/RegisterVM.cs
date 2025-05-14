using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [RegularExpression("^.*\\.(png|jpg|pdf)$")]
        public string? ImgProfile { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        [Required]
        public string SelectedRole { get; set; }
        public List<string> Roles { get; set; }
    }
}
