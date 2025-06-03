using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IScheduleServices
    {
        Task<IEnumerable<DoctorSchedule>> GetAllAsync(int id);
        Task<DoctorSchedule?> GetByIdAsync(int id);
        Task AddAsync(DoctorSchedule schedule);
        Task UpdateAsync(DoctorSchedule schedule);
        Task DeleteAsync(int id);
    }
}
