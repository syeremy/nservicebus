using FireOnWheels.Messages;
using FireOnWheels.Saga;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBus.Testing;

namespace FireOnWheels.Tests
{
    [TestClass]
    public class SagaTest
    {
        [TestMethod]
        public void ProcessOrderSaga_SendProcessOrderCommand_WhenPlanOrderCommandSent()
        {
            Test.Saga<ProcessOrderSaga>()
                    .ExpectSend<PlanOrderCommand>()
                .When((saga, context) => saga.Handle(new ProcessOrderCommand(), context));
        }

        [TestMethod]
        public void ProcessOrderSaga_SendDispatchOrderCommand_WhenOrderDispatchedMessageReceived()
        {
            Test.Saga<ProcessOrderSaga>()
                    .ExpectReplyToOriginator<OrderProcessedMessage>()
                .WhenHandling<IOrderDispatchedMessage>()
                    .AssertSagaCompletionIs(true);
        }
    }
}
