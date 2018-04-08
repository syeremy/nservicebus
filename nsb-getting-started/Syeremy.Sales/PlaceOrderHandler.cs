using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Commands;
using Syeremy.Messages.Events;

namespace Syeremy.Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            
            //throw new Exception("BOOM");

            var orderPlaced = new OrderPlaced
            {
                OrderId =  message.OrderId
            };

            await context.Publish(orderPlaced);
        }
    }
}