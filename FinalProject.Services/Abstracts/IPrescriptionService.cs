using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IPrescriptionService
    {
        public Task<Perscribtion> GetByAppointmentIdAsync(int id);
        public Task<int> Create(Perscribtion perscribtion);
    }
}
