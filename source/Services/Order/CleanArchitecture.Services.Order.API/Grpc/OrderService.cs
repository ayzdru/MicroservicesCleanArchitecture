using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace CleanArchitecture.Services.Order.API.Grpc
{
    public class OrderService : Order.OrderBase, ICapSubscribe
    {
        [CapSubscribe("xxx.services.show.time")]
        public void CheckReceivedMessage(DateTime datetime)
        {

        }
    }
}
