using FinalProject.Infrastructure.Bases;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepositry<T> Repositry<T>() where T : class;
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}
