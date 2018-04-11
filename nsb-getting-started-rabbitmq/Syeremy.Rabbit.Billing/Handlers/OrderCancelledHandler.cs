using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.Billing.Handlers
{
    public class OrderCancelledHandler : IHandleMessages<OrderCancelled>
    {
        static ILog logger = LogManager.GetLogger<OrderCancelledHandler>();

        public async Task Handle(OrderCancelled message, IMessageHandlerContext context)
        {
            logger.Info($"Received OrderCancelled, OrderId = {message.OrderId} - reembursing credit card");
            await Task.CompletedTask;
        }
    }
}