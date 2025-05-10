using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface INurseServices
    {
        public IQueryable<Nurse> GetAll();
        public Task<Nurse> GetById(int id);
        public Task<int> Create(Nurse nurse);
        public string Edit(Nurse nurse);
        public Task<string> Delete(int id);
    }
}
