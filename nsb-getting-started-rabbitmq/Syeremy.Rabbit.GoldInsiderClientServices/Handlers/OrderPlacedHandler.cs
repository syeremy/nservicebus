using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.GoldInsiderClientServices.Handlers
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");
            
            
            var orderBilled = new OrderBilled
            {
                OrderId =  message.OrderId
            };

            await context.Publish(orderBilled);
        }
    }
}