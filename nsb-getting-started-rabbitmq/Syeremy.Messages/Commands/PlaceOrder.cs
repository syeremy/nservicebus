﻿using NServiceBus;

namespace Syeremy.Messages.Commands
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
    }
}