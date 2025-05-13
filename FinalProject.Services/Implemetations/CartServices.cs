using FinalProject.Data.Models.PaymentModels;
using FinalProject.Infrastructure.UnitOfWorks;

namespace FinalProject.Services.Implemetations
{
    public class CartServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Create(Cart cart)
        {
            var final = await _unitOfWork.Repositry<Cart>().Create(cart);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return cart.Id;
            }
            else
                return 0;
        }
    }
}
