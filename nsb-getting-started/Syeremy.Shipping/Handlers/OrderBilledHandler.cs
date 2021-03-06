﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Events;

namespace Syeremy.Shipping.Handlers
{
    public class OrderBilledHandler : IHandleMessages<OrderBilled>
    {
        static readonly ILog log = LogManager.GetLogger<OrderBilledHandler>();

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.OrderId} - Preparing Shipment!..");

            await Task.CompletedTask;
        }
    }
}