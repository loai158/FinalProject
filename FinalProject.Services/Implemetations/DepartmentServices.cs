using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task AddDepartmentAsync(Department department)
        {
            var result = await _unitOfWork.Repositry<Department>().Create(department);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dept = await _unitOfWork.Repositry<Department>().GetOne(d => d.Id == id);
            if (dept != null)
            {
                _unitOfWork.Repositry<Department>().Delete(dept);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<int> findDeptByDocId(int doctorId)
        {
            Doctor doctor = await _unitOfWork.Repositry<Doctor>().GetOne(e => e.Id == doctorId);

            return doctor.DepartmentId;
        }

        public IQueryable<Department> getAll()
        {
            return _unitOfWork.Repositry<Department>().Get();
        }

        public async Task<Department> GetByIdAsync(int id)
        {

            var dept = await _unitOfWork.Repositry<Department>().GetOne(d => d.Id == id);
            if (dept == null)
                throw new Exception("no dept");
            return dept;
        }

        public async Task UpdateAsync(Department department)
        {
            _unitOfWork.Repositry<Department>().Edit(department);
            await _unitOfWork.CompleteAsync();
        }
    }
}
