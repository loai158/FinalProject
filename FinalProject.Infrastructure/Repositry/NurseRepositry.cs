using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;

namespace FinalProject.Infrastructure.Repositry
{
    public class NurseRepositry : GenericRepositry<Nurse>, INurseRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public NurseRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
