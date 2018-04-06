using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.Contracts;

namespace FireOnWheels.Monitoring
{
    public class CustomCheckFailedHandler: IHandleMessages<CustomCheckFailed>
    {
        public async Task Handle(CustomCheckFailed message, IMessageHandlerContext context)
        {
            //notify
        }
    }
}
