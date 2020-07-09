using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace CleanArchitecture.Services.Payment.API.Grpc
{
    public class PaymentService : Payment.PaymentBase, ICapSubscribe
    {
        [CapSubscribe("xxx.services.show.time")]
        public void CheckReceivedMessage(DateTime datetime)
        {
        }
    }
}
