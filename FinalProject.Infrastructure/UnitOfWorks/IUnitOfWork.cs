using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.IRepositry;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public IDoctorRepositry DoctorRepositry { get; }
        public IPatientRepositry PatientRepositry { get; }
        public INurseRepositry NurseRepositry { get; }
        public IRegisterApplyRepositoey RegisterApplyRepositoey { get; }
        public IMessageRepository MessageRepository { get; }

        IGenericRepositry<T> Repositry<T>() where T : class;
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}
