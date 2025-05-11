using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IQueryable<Appointment> GetAll()
        {
            var Appointments = _unitOfWork.Repositry<Appointment>().Get(includes: [d => d.Perscribtions, x => x.Department, z => z.Patient, p => p.Doctor]);
            return Appointments;
            throw new NotImplementedException();
        }
    }
}
