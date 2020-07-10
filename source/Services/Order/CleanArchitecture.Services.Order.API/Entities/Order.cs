using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Order.API.Entities
{
    public class Order : BaseEntity
    {
        public Order(Guid id, double totalAmount)
        {
            Id = id;
            TotalAmount = totalAmount;
        }

        public double TotalAmount { get; set; }
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
        }
    }
}
