using FinalProject.Core.Feature.Cart.Requests;
using FinalProject.Core.Feature.Cart.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Mapping
{
    public static class CartMapProfile
    {
        public static Cart CreateToCart(this CartRequest cartRequest)
        {
            return new Cart()
            {
                ApplicationUserId= cartRequest.ApplicationUserId,
                DoctorId=cartRequest.DoctorId,
                AppointmentId=(int)cartRequest.AppointmentId,
                PaymentType = (Cart.TypePayment)cartRequest.PaymentType
            };
        }

        public static GetAllResponce GetAll(this Cart cart)
        {
            return new GetAllResponce()
            {
                ApplicationUserId=cart.ApplicationUserId,
                Id=cart.Id,
                DocImage=cart.Doctor?.Image,
                DocName=cart.Doctor?.Name,
                DocPhone=cart.Doctor?.Phone,
               AppointDate=cart.Appointment.Date,
                PaymentType = cart.PaymentType.ToString()
            };
        }
    }
}
