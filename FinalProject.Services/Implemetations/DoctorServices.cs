using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

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

        public async Task<Doctor> GetById(int id)
        {
            var doctor = await _unitOfWork.Repositry<Doctor>().GetOne(c => c.Id == id, includes: [a => a.Appointments, ds => ds.DoctorSchedules, dd => dd.Department], tracked: false);
            return doctor;
        }

        public async Task<bool> IsDoctorNameExists(string name)
        {
            return await _unitOfWork.Repositry<Doctor>().Exist(d => d.Name == name);

        }
    }
}
