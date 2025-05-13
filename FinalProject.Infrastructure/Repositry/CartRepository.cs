using FinalProject.Data.Models.PaymentModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;


namespace FinalProject.Infrastructure.Repositry
{
    public class CartRepository : GenericRepositry<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
