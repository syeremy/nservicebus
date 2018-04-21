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
                OrderId =  message.OrderId,
                ClientId = message.ClientId
            };
            
            var publishOptions = new PublishOptions();
            publishOptions.SetHeader("insider", GetInsiderProgramValue(message));
            publishOptions.SetHeader("membership", GetMembership(message));

            // -- Echange : amq.topic, Queue : Syeremy-Messages-Events-OrderPlaced -- binding needed.
            await context.Publish(orderPlaced, publishOptions);

        }
        
        private string GetInsiderProgramValue(PlaceOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ? "1" : "0";
        }

        private string GetMembership(PlaceOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ||
                   cancelOrder.ClientId.Equals("AnotherSuperImportantClientLtd")
                ? "gold"
                : "silver";
        }
    }
}