using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Basket.API.ApiModels
{
    public class BasketItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
