using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;

namespace FinalProject.Infrastructure.Repositry
{
    public class PreviousConditionRepositry : GenericRepositry<PreviousCondition>, IPreviousConditionRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public PreviousConditionRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
