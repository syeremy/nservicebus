using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Commands;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.Sales.Handlers
{
    public class CancelOrderHandler: IHandleMessages<CancelOrder>
    {
        static ILog logger = LogManager.GetLogger<CancelOrderHandler>();

        public async Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            logger.Info($"Received CancelOrder, OrderId = {message.OrderId}");

            // This is normally where some business logic would occur
            
            var orderCancelled = new OrderCancelled
            {
                OrderId = message.OrderId
            };
            
            await context.Publish(orderCancelled);
        }
    }
}