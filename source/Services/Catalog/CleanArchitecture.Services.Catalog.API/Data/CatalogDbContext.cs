using CleanArchitecture.Services.Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Catalog.API.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
              : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
