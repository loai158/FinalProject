using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IMedicineServices
    {
        public Task<Medicine> GetByName(string name);
        public Task<Medicine> GetById(int id);
        public Task<int> Create(Medicine medicine);
        public Task<string> Add(Medicine Medicine);
        public Task<string> Update(Medicine Medicine);
    }
}
