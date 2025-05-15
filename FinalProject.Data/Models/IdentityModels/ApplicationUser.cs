
using FinalProject.Data.Models.AppModels;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Data.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ImgProfile { get; set; }
        public Doctor? DoctorProfile { get; set; }
        public Patient? PatientProfile { get; set; }
        public Nurse? NurseProfile { get; set; }
    }
}
