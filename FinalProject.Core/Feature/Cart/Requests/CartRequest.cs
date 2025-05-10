

namespace FinalProject.Core.Feature.Cart.Requests
{
    public class CartRequest 
    {
        public string ApplicationUserId { get; set; }   
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public TypePayment PaymentType { get; set; }

    }

    public enum TypePayment
    {
        Online,
        Cash
    }

}
