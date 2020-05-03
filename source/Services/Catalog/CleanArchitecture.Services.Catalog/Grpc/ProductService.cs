using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Services.Catalog.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Catalog.Grpc
{
    [Authorize]    
    public class ProductService : Product.ProductBase
    {
        private readonly CatalogDbContext _catalogDbContext;
        public ProductService(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public override Task<ProductsResponse> GetProducts(Empty request, ServerCallContext context)
        {
            var productsResponse = new ProductsResponse();
            var products = _catalogDbContext.Products.Select(q => new ProductResponse() { Name = q.Name, Price = q.Price }).ToList();
            productsResponse.Products.AddRange(products);
            return Task.FromResult(productsResponse); 
        }
    }
}
