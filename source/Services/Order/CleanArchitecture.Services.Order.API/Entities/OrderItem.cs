using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Order.API.Entities
{
    public class OrderItem : BaseEntity
    {
        public Order Order { get; private set; }
        public Guid OrderId { get; private set; }
        public Product Product { get; private set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; private set; }
        public double TotalAmount { get; set; }
    }
}
