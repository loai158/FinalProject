using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.IRepositry;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork:IDisposable
    {

        public ICartRepository CartRepository { get; }
        IGenericRepositry<T> Repositry<T>() where T : class;
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}
