﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using FireOnWheels.Messages;
using NServiceBus;
using Order = FireOnWheels.Rest.Models.Order;

namespace FireOnWheels.Rest.Controllers
{
    public class DispatchController : ApiController
    {
        private readonly IEndpointInstance endpoint;

        public DispatchController(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint;
        }
        public async Task Post(Order order)
        {
            await endpoint.Send(new ProcessOrderCommand
            {
                OrderId = Guid.NewGuid(),
                AddressFrom = order.AddressFrom,
                AddressTo = order.AddressTo,
                Price = order.Price,
                Weight = order.Weight
            }).ConfigureAwait(false);
        }

    }
}