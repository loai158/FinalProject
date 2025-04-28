using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IDoctorServices
    {
        public  IQueryable<Doctor> GetAll();
    }
}
