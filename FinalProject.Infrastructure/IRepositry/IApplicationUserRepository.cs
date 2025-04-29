using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Infrastructure.IRepositry
{
   public interface IApplicationUserRepository:IGenericRepositry<ApplicationUser>
    {
    }
}
