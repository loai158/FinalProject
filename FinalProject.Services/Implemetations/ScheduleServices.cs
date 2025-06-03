using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class ScheduleServices : IScheduleServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task AddAsync(DoctorSchedule schedule)
        {
            await _unitOfWork.Repositry<DoctorSchedule>().Create(schedule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var schedule = await _unitOfWork.Repositry<DoctorSchedule>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<DoctorSchedule>().Delete(schedule);
                _unitOfWork.Repositry<DoctorSchedule>().Commit();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }

        public async Task<IEnumerable<DoctorSchedule>> GetAllAsync(int id)
        {
            var allSchedule = _unitOfWork.Repositry<DoctorSchedule>().Get(filter: d => d.DoctorId == id);
            return allSchedule ?? Enumerable.Empty<DoctorSchedule>();
        }

        public Task<DoctorSchedule?> GetByIdAsync(int id)
        {
            var schedule = _unitOfWork.Repositry<DoctorSchedule>().GetOne(filter: d => d.Id == id);
            return schedule;
        }

        public async Task UpdateAsync(DoctorSchedule schedule)
        {
            _unitOfWork.Repositry<DoctorSchedule>().Edit(schedule);
            await _unitOfWork.CompleteAsync();

        }
    }
}
