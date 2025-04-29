using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface INurseServices
    {
        public Task<Nurse> GetById(int id);
    }
}
