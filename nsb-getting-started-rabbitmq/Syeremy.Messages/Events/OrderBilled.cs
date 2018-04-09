using NServiceBus;

namespace Syeremy.Messages.Events
{
    public class OrderBilled : IEvent
    {
        public string OrderId { get; set; }
    }
}