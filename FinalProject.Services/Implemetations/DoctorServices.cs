using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services.Implemetations
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<int> Create(Doctor doctor)
        {


            var final = await _unitOfWork.Repositry<Doctor>().Create(doctor);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return doctor.Id;
            }
            else
                return 0;
        }

        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var doctor = await _unitOfWork.Repositry<Doctor>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<Doctor>().Delete(doctor);
                _unitOfWork.Repositry<Doctor>().Commit();
                await _unitOfWork.CommitTransactionAsync();
                return "success";
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return "faild";

            }
        }

        public string Edit(Doctor doctor)
        {
            _unitOfWork.Repositry<Doctor>().Edit(doctor);
            _unitOfWork.Repositry<Doctor>().Commit();
            return "success";
        }

        public IQueryable<Doctor> GetAll()
        {
            var result = _unitOfWork.Repositry<Doctor>().Get(includes: [d => d.Department, a => a.DoctorSchedules]);
            return result;
        }

        public IQueryable<Doctor> GetByDeptId(int id)
        {
            var result = _unitOfWork.Repositry<Doctor>().Get(includes: [e => e.Department], filter: d => d.DepartmentId == id);
            return result;
        }

        public async Task<Doctor> GetById(int id)
        {
            var doctor = await _unitOfWork.Repositry<Doctor>().GetOne(c => c.Id == id, includes: [a => a.Appointments, ds => ds.DoctorSchedules, dd => dd.Department], tracked: false);
            return doctor;
        }

        public async Task<int> GetTotalCount(string query)
        {
            var queryable = _unitOfWork.Repositry<Doctor>().Get().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                queryable = queryable.Where(d => d.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                                               || d.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            return await queryable.CountAsync();
        }

        public async Task<bool> IsDoctorNameExists(string name)
        {
            return await _unitOfWork.Repositry<Doctor>().Exist(d => d.Name == name);

        }

        public async Task update(int id)
        {
            var schedule = await _unitOfWork.Repositry<DoctorSchedule>().GetOne(d => d.Id == id);
            if (schedule != null)
            {
                schedule.IsAvailable = false;
            }
            _unitOfWork.Repositry<DoctorSchedule>().Edit(schedule);
            await _unitOfWork.CompleteAsync();
        }
    }
}
