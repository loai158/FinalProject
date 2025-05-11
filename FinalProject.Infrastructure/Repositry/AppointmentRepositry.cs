using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;

namespace FinalProject.Infrastructure.Repositry
{
    public class AppointmentRepositry : GenericRepositry<Appointment>, IAppointmentService
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
