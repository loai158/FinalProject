using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IDoctorServices
    {
        public  IQueryable<Doctor> GetAll();
        public Task<int> Create(Doctor doctor);
        public Task<bool> IsDoctorNameExists(string name);
    }
}
