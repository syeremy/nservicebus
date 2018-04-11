using NServiceBus;

namespace Syeremy.Messages.Commands
{
    public class CancelOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}