using NServiceBus;

namespace Syeremy.Messages.Events
{
    public class OrderCancelled: IEvent
    {
        public string OrderId { get; set; }
    }
}