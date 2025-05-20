using FinalProject.Data.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models.PaymentModels
{
   public class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        public int ?DoctorId { get; set; }
        public Doctor ?Doctor { get; set; }
     
        public double ?Price { get; set; }
    }
}
