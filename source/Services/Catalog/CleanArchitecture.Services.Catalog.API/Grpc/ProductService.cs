using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Services.Catalog.API.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Catalog.API.Grpc
{   
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
            var products = _catalogDbContext.Products.Select(q => new ProductResponse() { Id=q.Id.ToString(), Name = q.Name, Description =q.Description, Price = q.Price }).ToList();
            productsResponse.Products.AddRange(products);
            return Task.FromResult(productsResponse); 
        }

        public override Task<ProductResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(_catalogDbContext.Products
                .Where(q => q.Id == Guid.Parse(request.ProductId))
                .Select(q => new ProductResponse()
                    {Id = q.Id.ToString(), Price = q.Price, Description = q.Description, Name = q.Name})
                .SingleOrDefault());
        }
    }
}
