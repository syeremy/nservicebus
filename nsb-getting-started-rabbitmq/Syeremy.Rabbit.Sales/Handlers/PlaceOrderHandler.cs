using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Commands;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.Sales.Handlers
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            
            var orderPlaced = new OrderPlaced
            {
                OrderId =  message.OrderId
            };

            // -- Echange : amq.topic, Queue : Syeremy-Messages-Events-OrderPlaced -- binding needed.
            await context.Publish(orderPlaced);

        }
    }
}