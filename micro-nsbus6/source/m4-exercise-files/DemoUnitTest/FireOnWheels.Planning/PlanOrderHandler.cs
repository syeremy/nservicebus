using System;
using System.Threading.Tasks;
using FireOnWheels.Messages;
using NServiceBus;

namespace FireOnWheels.Planning
{
    public class PlanOrderHandler: IHandleMessages<PlanOrderCommand>
    {
        public async Task Handle(PlanOrderCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OrderId {message.OrderId} planned");
            //Do planning
            await context.Reply<IOrderPlannedMessage>(msg => { }).ConfigureAwait(false);
        }
    }
}
