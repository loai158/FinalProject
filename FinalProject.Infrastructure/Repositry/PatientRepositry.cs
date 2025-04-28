using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;

namespace FinalProject.Infrastructure.Repositry
{
    public class PatientRepositry : GenericRepositry<Patient>, IPatientRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public PatientRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
