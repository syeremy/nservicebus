using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.Shipping.Handlers
{
    public class OrderCancelledHandler: IHandleMessages<OrderCancelled>
    {
        static ILog logger = LogManager.GetLogger<OrderCancelledHandler>();

        public async Task Handle(OrderCancelled message, IMessageHandlerContext context)
        {
            logger.Info($"Received OrderCancelled, OrderId = {message.OrderId} - shipment cancelled");
            await Task.CompletedTask;
        }
    }
}