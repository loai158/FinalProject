using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IPreviousConditionServices
    {
        public Task<string> Add(PreviousCondition previousCondition);
        public Task<PreviousCondition> FindByName(int value);
        public IQueryable<PreviousCondition> GetAll();
        public Task<string> Update(PreviousCondition previousCondition);
        Task<string> Delete(int id);
        public Task<string> Save();
    }
}
