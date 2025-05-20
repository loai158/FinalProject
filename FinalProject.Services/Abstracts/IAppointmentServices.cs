using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IAppointmentServices
    {
        public IQueryable<Appointment> GetAll(string userId);
        public IQueryable<Appointment> GetAll();
        public Task<int> Create(Appointment appointment);
        public Task<Appointment> GetById(int id);
        public string Edit(Appointment appointment);
        public Task<string> Delete(int id);

        public Task<int?> GetPatientIdFromUserAsync(string userId);
    }
}
