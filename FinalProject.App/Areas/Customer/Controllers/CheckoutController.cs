using FinalProject.Data.Models.PaymentModels;
using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CheckoutController : Controller
    {
         private readonly IUnitOfWork _unitOfWork;

        public CheckoutController(IUnitOfWork unitOfWork)
        {
             this._unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Success(int orderId)
        {
            var order =await _unitOfWork.OrderRepository.GetOne(e => e.Id == orderId);
            if (order != null)
            {
                var services = new SessionService();
                var session = services.Get(order.SessionId);

                order.PaymentStripeId = session.PaymentIntentId;
                order.Status = OrderStatus.Processing;
                order.PaymentStatus = true;
                order.IsShiped = false;

                await _unitOfWork.CompleteAsync();
            }

            TempData["Success"] = "Checkout Successfully!";
            return View();
        }



        public IActionResult Cancel()
        {
            TempData["Success"] = "Checkout Canceled Successfully !";
            return View();
        }

    }
}
