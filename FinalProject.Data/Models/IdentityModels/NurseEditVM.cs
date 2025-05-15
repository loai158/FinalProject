using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class NurseEditVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Image { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Email { get; set; }


        public int DepartmentId { get; set; }
    }
}
