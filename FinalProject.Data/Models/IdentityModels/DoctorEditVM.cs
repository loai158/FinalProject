using FinalProject.Data.Models.AppModels;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models.IdentityModels
{
    public class DoctorEditVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Image { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Email { get; set; }

        [Required]
        public string Details { get; set; }

        public decimal? IntialPrice { get; set; }
        public decimal? FollowUpPrice { get; set; }

        public Gender Gender { get; set; }

        public int DepartmentId { get; set; }
    }

}
