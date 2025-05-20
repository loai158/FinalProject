using FinalProject.Data.Models.PaymentModels;
using FinalProject.Infrastructure.Bases;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Infrastructure.Repositry
{
   public class OrderItemRepository: GenericRepositry<OrderItem>,IOrderItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateRange(IEnumerable<OrderItem> orderItems)
        {
            dbContext.AddRange(orderItems);
        }
    }
}
