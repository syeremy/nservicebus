using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.Contracts;

namespace FireOnWheels.Monitoring
{
    public class MessageFailedHandler: IHandleMessages<MessageFailed>
    {
        public async Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            string failedMessageId = message.FailedMessageId;
            string exceptionMessage = message.FailureDetails.Exception.Message;

            //notify
        }
    }
}
