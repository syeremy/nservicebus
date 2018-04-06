using System.Threading.Tasks;
using FireOnWheels.Messages;
using NServiceBus;

namespace FireOnWheels.OrderActivity
{
    public class OrderActivityHandler: 
        IHandleMessages<IOrderActivityEvent>
    {
        public async Task Handle(IOrderActivityEvent message, 
            IMessageHandlerContext context)
        {
            //store activity
        }
    }
}
