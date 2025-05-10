using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Infrastructure.Repositry
{
  public  class DoctorRepositry : GenericRepositry<Doctor>, IDoctorRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
