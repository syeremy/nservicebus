using NServiceBus;

namespace Syeremy.Messages.Events
{
    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
    }
    
}