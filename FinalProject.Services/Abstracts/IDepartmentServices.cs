using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IDepartmentServices
    {
        public IQueryable<Department> getAll();
    }
}
