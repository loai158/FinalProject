using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class NurseServices : INurseServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public NurseServices(IUnitOfWork unitOfWork  )
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Nurse> GetById(int id)
        {
            var nurse = await _unitOfWork.Repositry<Nurse>().GetOne(n=>n.Id == id, includes: [d=>d.Department]);
                return nurse  ;
        }

    }
}
