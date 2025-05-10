using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Feature.Cart.Responces
{
 public class GetAllResponce
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string ?DocName { get; set; }
        public string ?DocPhone { get; set; }
        public string ?DocImage { get; set; }
        public DateOnly ?AppointDate { get; set; }
        public string PaymentType { get; set; }

 
}
}
