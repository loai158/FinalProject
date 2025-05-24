using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models.IdentityModels
{
 

    public class ApplyRequest
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; }
        [Required]
        public DateTime ApplyDate { get; set; }

        [RegularExpression("^.*\\.(png|jpg)$")]
        public string ? FilePdf { get; set; }
    }
    public enum RoleType { 
        Doctor,
        Nurse 
    }
}
