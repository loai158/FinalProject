using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;

namespace FinalProject.Infrastructure.Repositry
{
    public class DoctorScheduleRepositry : GenericRepositry<DoctorSchedule>, IDoctorScheduleRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorScheduleRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
