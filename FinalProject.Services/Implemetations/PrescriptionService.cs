using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    internal class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrescriptionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Perscribtion> GetByAppointmentIdAsync(int id)
        {
            var result = await _unitOfWork.Repositry<Perscribtion>().GetOne(
                                f => f.AppointmentId == id,
                                includes: [s => s.PerscribtionMedicines]
                                );
            return result;
        }
        public async Task<int> Create(Perscribtion perscribtion)
        {

            var final = await _unitOfWork.Repositry<Perscribtion>().Create(perscribtion);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return perscribtion.Id;
            }
            else
                return 0;
        }
    }
}
