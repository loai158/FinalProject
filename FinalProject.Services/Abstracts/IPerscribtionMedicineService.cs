using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IPerscribtionMedicineService
    {
        public void create(PerscribtionMedicine perscribtionMedicine);
        public IQueryable<PerscribtionMedicine> GetAll();
        public string Edit(PerscribtionMedicine perscribtionMedicine);
        public Task<string> Delete(int id);
    }
}
