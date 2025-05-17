using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class PreviousConditionServices : IPreviousConditionServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PreviousConditionServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<string> Add(PreviousCondition previousCondition)
        {
            var result = await _unitOfWork.Repositry<PreviousCondition>().Create(previousCondition);
            await _unitOfWork.CompleteAsync();
            if (result == "success")
            {
                return "success";
            }
            else
                return "faild";
        }

        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var previousCondition = await _unitOfWork.Repositry<PreviousCondition>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<PreviousCondition>().Delete(previousCondition);
                _unitOfWork.Repositry<PreviousCondition>().Commit();
                await _unitOfWork.CommitTransactionAsync();
                return "success";
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return "faild";

            }
        }

        public async Task<PreviousCondition> FindByName(int value)
        {
            if (Enum.IsDefined(typeof(Name), value))
            {
                var enumValue = (Name)value; // تحويل int إلى التعداد
                var result = await _unitOfWork.Repositry<PreviousCondition>().GetOne(filter: e => e.Name == enumValue);
                return result; // إرجاع السلسلة النصية للتعداد
            }
            return null;
        }

        public IQueryable<PreviousCondition> GetAll()
        {
            return _unitOfWork.Repositry<PreviousCondition>().Get();
        }

        public async Task<string> Save()
        {
            await _unitOfWork.CompleteAsync();
            return "";
        }

        public async Task<string> Update(PreviousCondition previousCondition)
        {
            var result = _unitOfWork.Repositry<PreviousCondition>().Edit(previousCondition);
            if (result == "success")
            {
                await _unitOfWork.CompleteAsync();
                return "success";
            }
            return "faild";
        }
    }
}
