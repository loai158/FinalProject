using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IPreviousMedicineServices
    {
        public Task<string> Add(PreviousMedicine previousMedicine);
        IQueryable<PreviousMedicine> GetAll();
        public Task<string> Update(PreviousMedicine previousMedicine);
        Task<PreviousMedicine> GetById(int id);
        Task<string> Delete(int id);
    }
}
