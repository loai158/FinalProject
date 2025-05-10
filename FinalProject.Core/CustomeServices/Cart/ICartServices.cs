using FinalProject.Core.Feature.Cart.Requests;
using FinalProject.Core.Feature.Cart.Responces;

namespace FinalProject.Services.Implemetations
{
    public interface ICartServices
    {
        public void AddCart(CartRequest cartRequest);
        IEnumerable<GetAllResponce> GetAll();
    }
}