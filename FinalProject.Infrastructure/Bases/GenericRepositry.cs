using FinalProject.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinalProject.Infrastructure.Bases
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        public DbSet<T> dbSet;
        private readonly ApplicationDbContext dbContext;
        public GenericRepositry(ApplicationDbContext dbContext)
        {
            dbSet = dbContext.Set<T>();
            this.dbContext = dbContext;
        }

        // CRUD
        public async Task<string> Create(T entity)
        {
            await dbSet.AddAsync(entity);
            return "success";
        }

        public async Task Create(List<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public string Edit(T entity)
        {
            dbSet.Update(entity);
            return "success";
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<T?> GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return await Get(filter, includes, tracked).FirstOrDefaultAsync();

        }
    }
}
