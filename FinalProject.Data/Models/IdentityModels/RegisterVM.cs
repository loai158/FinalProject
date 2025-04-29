using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string ConfirmPassword { get; set; }
        [RegularExpression("^.*\\.(png|jpg|pdf)$")]
        public string? ImgProfile { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
