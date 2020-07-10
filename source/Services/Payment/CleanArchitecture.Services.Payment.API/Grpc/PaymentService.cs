using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Services.Payment.API.Data;
using DotNetCore.CAP;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanArchitecture.Services.Payment.API.Grpc
{
    public class PaymentService : Payment.PaymentBase, ICapSubscribe
    {
        private readonly PaymentDbContext _paymentDbContext;
        private readonly ICapPublisher _capBus;


        public PaymentService(PaymentDbContext paymentDbContext, ICapPublisher capBus)
        {
            _paymentDbContext = paymentDbContext;
            _capBus = capBus;
        }

        [CapSubscribe("AddPayment")]
        public void AddPayment(Guid orderId)
        {
            using (var transaction = _paymentDbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                _paymentDbContext.Payments.Add(new Entities.Payment() { OrderId = orderId });
                _paymentDbContext.SaveChanges();
                transaction.Commit();
            }

        }

    }
}
