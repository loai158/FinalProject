using FinalProject.Data.Models.AppModels;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class PatientEditVm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string? Image { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
