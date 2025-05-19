using FinalProject.Data.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models.PaymentModels
{
  public  class Order
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsShiped { get; set; }
        public bool PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripeId { get; set; }
    }

    public enum OrderStatus
    {
        Canceled = 0,
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4
    }
}
 
