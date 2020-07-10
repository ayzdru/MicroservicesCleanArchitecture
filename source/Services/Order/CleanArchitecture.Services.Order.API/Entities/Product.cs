using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Order.API.Entities
{
    public class Product : BaseEntity
    {
        public Product(Guid id, string name, string description, double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
