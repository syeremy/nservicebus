using System;
using System.Threading.Tasks;
using FireOnWheels.Dispatch.Helper;
using FireOnWheels.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace FireOnWheels.Dispatch
{
    public class DispatchOrderHandler:IHandleMessages<DispatchOrderCommand>
    {
        private static readonly ILog Logger = 
            LogManager.GetLogger(typeof(DispatchOrderHandler));

        public async Task Handle(DispatchOrderCommand message, IMessageHandlerContext context)
        {
            throw new Exception();
            Logger.InfoFormat("Order received! To address: {0}", message.AddressTo);
            EmailSender.SendEmailToDispatch(message);

            await context.Reply<IOrderDispatchedMessage>(e => { }).ConfigureAwait(false);
        }
    }
}
