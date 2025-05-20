using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class PerscribtionMedicineService : IPerscribtionMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PerscribtionMedicineService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async void create(PerscribtionMedicine perscribtionMedicine)
        {
            await _unitOfWork.Repositry<PerscribtionMedicine>().Create(perscribtionMedicine);
            await _unitOfWork.CompleteAsync();

        }
        public IQueryable<PerscribtionMedicine> GetAll()
        {
            var result = _unitOfWork.Repositry<PerscribtionMedicine>().Get();

            return result;
        }
        public string Edit(PerscribtionMedicine perscribtionMedicine)
        {
            _unitOfWork.Repositry<PerscribtionMedicine>().Edit(perscribtionMedicine);
            _unitOfWork.Repositry<PerscribtionMedicine>().Commit();
            return "success";
        }
        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var perscribtionMedicine = await _unitOfWork.Repositry<PerscribtionMedicine>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<PerscribtionMedicine>().Delete(perscribtionMedicine);
                _unitOfWork.Repositry<PerscribtionMedicine>().Commit();
                await _unitOfWork.CommitTransactionAsync();
                return "success";
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return "faild";

            }
        }
    }
}
