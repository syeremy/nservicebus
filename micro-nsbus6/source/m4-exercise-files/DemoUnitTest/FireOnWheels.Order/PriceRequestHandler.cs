using System.Threading.Tasks;
using FireOnWheels.Dispatch.Helper;
using FireOnWheels.Messages;
using NServiceBus;

namespace FireOnWheels.Dispatch
{
    public class PriceRequestHandler: IHandleMessages<PriceRequest>
    {
        public async Task Handle(PriceRequest message, IMessageHandlerContext context)
        {
            await context.Reply(new PriceResponse {Price = PriceCalculator.GetPrice(message)})
                .ConfigureAwait(false);
        }
    }
}
