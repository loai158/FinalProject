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

        public async Task<int> findDeptByDocId(int doctorId)
        {
            Doctor doctor = await _unitOfWork.Repositry<Doctor>().GetOne(e => e.Id == doctorId);

            return doctor.DepartmentId;
        }

        public IQueryable<Department> getAll()
        {
            return _unitOfWork.Repositry<Department>().Get();
        }
    }
}
