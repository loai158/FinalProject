using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject.Infrastructure.UnitOfWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepositry<T> Repositry<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepositry<T>;
            }
            var repository = new GenericRepositry<T>(_context);
            _repositories[typeof(T)] = repository;
            return repository;
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

    }
}
