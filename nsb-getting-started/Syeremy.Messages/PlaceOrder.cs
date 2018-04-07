using NServiceBus;

namespace Syeremy.Messages
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}