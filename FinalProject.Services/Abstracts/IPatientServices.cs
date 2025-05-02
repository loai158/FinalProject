using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IPatientServices
    {
        public IQueryable<Patient> GetAll(); 
        public Task<int> Create(Patient patient);

    }

}
