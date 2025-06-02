using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IDepartmentServices
    {
        public IQueryable<Department> getAll();
        public Task<int> findDeptByDocId(int doctorId);
        Task AddDepartmentAsync(Department department);
        Task<Department> GetByIdAsync(int id);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
    }
}
