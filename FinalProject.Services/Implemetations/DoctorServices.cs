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
        public IQueryable<Doctor> GetAll()
        {
            var result = _unitOfWork.Repositry<Doctor>().Get(includes: [c => c.Department]);
            return result;
        }
    }
}
