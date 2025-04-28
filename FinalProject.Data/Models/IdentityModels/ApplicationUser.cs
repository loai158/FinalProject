
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Data.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
    }
}
