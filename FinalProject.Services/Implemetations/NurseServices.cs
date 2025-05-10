using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class NurseServices : INurseServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public NurseServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Nurse> GetById(int id)
        {
            var nurse = await _unitOfWork.Repositry<Nurse>().GetOne(n => n.Id == id, includes: [d => d.Department]);
            return nurse;
        }
        public IQueryable<Nurse> GetAll()
        {
            var result = _unitOfWork.Repositry<Nurse>().Get(includes: [d => d.Department]);
            return result;
        }
        public async Task<int> Create(Nurse nurse)
        {
            var result = await _unitOfWork.Repositry<Nurse>().Exist(p => p.Name == nurse.Name);
            if (result)
            {
                //name exist
                return -1;
            }

            var final = await _unitOfWork.Repositry<Nurse>().Create(nurse);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return nurse.Id;
            }
            else
                return 0;
        }
        public string Edit(Nurse nurse)
        {
            _unitOfWork.Repositry<Nurse>().Edit(nurse);
            _unitOfWork.Repositry<Nurse>().Commit();
            return "success";
        }
        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var nurse = await _unitOfWork.Repositry<Nurse>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<Nurse>().Delete(nurse);
                _unitOfWork.Repositry<Nurse>().Commit();
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
