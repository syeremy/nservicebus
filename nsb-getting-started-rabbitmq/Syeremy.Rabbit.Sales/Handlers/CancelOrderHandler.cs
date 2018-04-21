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
            logger.Info($"Received CancelOrder, OrderId = {message.OrderId}  ClientId = {message.ClientId}");

            // This is normally where some business logic would occur
            
            var orderCancelled = new OrderCancelled
            {
                OrderId = message.OrderId,
                ClientId = message.ClientId
            };
            
            var publishOptions = new PublishOptions();
            publishOptions.SetHeader("insider", GetInsiderProgramValue(message));
            publishOptions.SetHeader("membership", GetMembership(message));
            
            await context.Publish(orderCancelled, publishOptions);
        }
        
        
        private string GetInsiderProgramValue(CancelOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ? "1" : "0";
        }

        private string GetMembership(CancelOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ||
                   cancelOrder.ClientId.Equals("AnotherSuperImportantClientLtd")
                ? "gold"
                : "silver";
        }
    }
}