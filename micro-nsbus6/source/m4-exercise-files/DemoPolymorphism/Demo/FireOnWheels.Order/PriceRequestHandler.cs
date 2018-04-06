using System.Threading.Tasks;
using FireOnWheels.Messages;
using FireOnWheels.Order.Helper;
using NServiceBus;

namespace FireOnWheels.Order
{
    public class PriceRequestHandler: IHandleMessages<PriceRequest>
    {
        public async Task Handle(PriceRequest message, IMessageHandlerContext context)
        {
            await context.Reply(new PriceResponse {Price = await PriceCalculator.GetPrice(message)})
                .ConfigureAwait(false);
        }
    }
}
