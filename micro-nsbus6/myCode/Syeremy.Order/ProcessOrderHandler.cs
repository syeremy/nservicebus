using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages;
using Syeremy.Order.Helper;

namespace Syeremy.Order
{
    public class ProcessOrderHandler : IHandleMessages<ProcessOrderCommand>
    {
        private static readonly ILog _logger = LogManager.GetLogger<ProcessOrderHandler>();
        
        public async Task Handle(ProcessOrderCommand message, IMessageHandlerContext context)
        {
            _logger.Info($"Message Received from address {message.AddressFrom} to address : {message.AddressTo}");

            await EmailSender.SendEmailToDispatch(message);
        }
    }

 
}