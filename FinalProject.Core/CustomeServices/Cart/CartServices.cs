using FinalProject.Core.Feature.Cart.Requests;
using FinalProject.Core.Feature.Cart.Responces;
using FinalProject.Core.Mapping;
using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FinalProject.Services.Implemetations
{

    public class CartServices : ICartServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<GetAllResponce> GetAll()
        {
            var carts = _unitOfWork.CartRepository.Get(includes: [e=>e.Doctor,e=>e.Appointment]).ToList();
            //var carts = new List<Cart>();
            return carts.Select(e => e.GetAll()).ToList();
        }
        public void AddCart(CartRequest cartRequest)
        {
            var cart = cartRequest.CreateToCart();

            _unitOfWork.CartRepository.Create(cart);
            _unitOfWork.CartRepository.Commit();
        }

   

    }
}
