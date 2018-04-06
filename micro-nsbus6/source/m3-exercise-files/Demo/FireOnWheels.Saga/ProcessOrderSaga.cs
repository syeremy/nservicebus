using System.Threading.Tasks;
using FireOnWheels.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace FireOnWheels.Saga
{
    public class ProcessOrderSaga : Saga<ProcessOrderSagaData>,
        IAmStartedByMessages<ProcessOrderCommand>,
        IHandleMessages<IOrderPlannedMessage>,
        IHandleMessages<IOrderDispatchedMessage>
    {
        private static readonly ILog Logger =
            LogManager.GetLogger(typeof(ProcessOrderSaga));

        protected override void ConfigureHowToFindSaga
            (SagaPropertyMapper<ProcessOrderSagaData> mapper)
        {
            mapper.ConfigureMapping<ProcessOrderCommand>(msg => msg.OrderId)
                .ToSaga(s => s.OrderId);
        }

        public async Task Handle(ProcessOrderCommand message, IMessageHandlerContext context)
        {
            Logger.InfoFormat("ProcessOrder command received. Starting saga for orderid {0}."
                , message.OrderId);
            Data.OrderId = message.OrderId;
            Data.AddressFrom = message.AddressFrom;
            Data.AddressTo = message.AddressTo;
            Data.Price = message.Price;
            Data.Weight = message.Weight;
            await context.Send(new PlanOrderCommand
            {
                OrderId = Data.OrderId,
                AddressTo = Data.AddressTo
            }).ConfigureAwait(false);
        }

        public async Task Handle(IOrderPlannedMessage message, IMessageHandlerContext context)
        {
            Logger.InfoFormat("Order {0} has been planned. Sending dispatch command.",
                Data.OrderId);
            await context.Send(new DispatchOrderCommand
            {
                AddressTo = Data.AddressTo,
                Weight = Data.Weight
            });
        }
        public async Task Handle(IOrderDispatchedMessage message,
            IMessageHandlerContext context)
        {
            Logger.InfoFormat("Order {0} has been dispatched. " +
                              "Notifying originator and ending saga.",
                Data.OrderId);
            await ReplyToOriginator(context, new OrderProcessedMessage())
                .ConfigureAwait(false);
            MarkAsComplete();
        }
    }
}
