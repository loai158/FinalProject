using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class MedicineServices : IMedicineServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Medicine> GetByName(string name)
        {
            var medicine = await _unitOfWork.Repositry<Medicine>().GetOne(m => m.Name.Contains(name));
            return medicine;
        }
        public async Task<int> Create(Medicine medicine)
        {

            var final = await _unitOfWork.Repositry<Medicine>().Create(medicine);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return medicine.Id;
            }
            else
                return 0;
        }
        public async Task<string> Add(Medicine medicine)
        {
            var result = await _unitOfWork.Repositry<Medicine>().Create(medicine);
            await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task<Medicine> GetById(int id)
        {
            return await _unitOfWork.Repositry<Medicine>().GetOne(m => m.Id == id);
        }

        public async Task<string> Update(Medicine Medicine)
        {
            _unitOfWork.Repositry<Medicine>().Edit(Medicine);
            await _unitOfWork.CompleteAsync();
            return "success";
        }
    }
}
