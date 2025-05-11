using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IAppointmentServices
    {
        public IQueryable<Appointment> GetAll();
    }
}
