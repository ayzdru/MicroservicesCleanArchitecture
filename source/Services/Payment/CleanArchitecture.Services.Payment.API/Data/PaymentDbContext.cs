using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Services.Payment.API.Entities;

namespace CleanArchitecture.Services.Payment.API.Data
{
    public class PaymentDbContext : DbContext
    {
        //public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        //    : base(options)
        //{ }
        public const string ConnectionString = "Server=(local);Database=PaymentDb;Trusted_Connection=True;";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        public DbSet<Entities.Payment> Payments { get; set; }
    }
}
