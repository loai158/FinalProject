using System.Linq.Expressions;

namespace FinalProject.Infrastructure.Bases
{
    public interface IGenericRepositry<T> where T : class
    {
        public Task<string> Create(T entity);

        public Task Create(List<T> entities);

        public string Edit(T entity);

        public void Delete(T entity);

        public void Delete(List<T> entities);

        public void Commit();

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public Task<T?> GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

    }
}
