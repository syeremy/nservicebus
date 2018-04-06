using System.Threading.Tasks;
using FireOnWheels.Messages;
using NServiceBus;

namespace FireOnWheels.Web.Handlers
{
    public class OrderProcessedEventHandler :
        IHandleMessages<OrderProcessedMessage>
    {
        public async Task Handle(OrderProcessedMessage message, IMessageHandlerContext context)
        {
        }
    }
}