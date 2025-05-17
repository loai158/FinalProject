using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class PreviousMedicineServices : IPreviousMedicineServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PreviousMedicineServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<string> Add(PreviousMedicine previousMedicine)
        {
            var result = await _unitOfWork.Repositry<PreviousMedicine>().Create(previousMedicine);
            await _unitOfWork.CompleteAsync();
            return result;
        }

        public IQueryable<PreviousMedicine> GetAll()
        {
            return _unitOfWork.Repositry<PreviousMedicine>().Get();
        }

        public async Task<PreviousMedicine> GetById(int id)
        {
            return await _unitOfWork.Repositry<PreviousMedicine>().GetOne(filter: m => m.Id == id);
        }
        public async Task<string> Update(PreviousMedicine previousMedicine)
        {
            var result = _unitOfWork.Repositry<PreviousMedicine>().Edit(previousMedicine);
            if (result == "success")
            {
                await _unitOfWork.CompleteAsync();
                return "success";
            }
            return "faild";
        }
        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var previousMedicine = await _unitOfWork.Repositry<PreviousMedicine>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<PreviousMedicine>().Delete(previousMedicine);
                _unitOfWork.Repositry<PreviousMedicine>().Commit();
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
