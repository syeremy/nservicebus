﻿using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.ClientUI
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Rabbit ClientUI";

            var endpointConfiguration = new EndpointConfiguration("Rabbit.ClientUI");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);

            // comment this line for Conventional Routing Topology
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Rabbit.Sales");
            routing.RouteToEndpoint(typeof(CancelOrder), "Rabbit.Sales");

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static int SendCount = 0;

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place a 20 orders that will fail, 'C' to cancel an order, 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:

                        for (int i = 0; i < 20; i++)
                        {
                            SendCount++;
                            // Instantiate the command
                            var command = new PlaceOrder
                            {
                                OrderId = SendCount.ToString()
                            };

                            log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                            await endpointInstance.Send(command).ConfigureAwait(false);
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }

                        break;

                    case ConsoleKey.C:
                        //Instantiate the command
                        var cancelCommand = new CancelOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        log.Info($"Sending CancelOrder command, OrderId = {cancelCommand.OrderId}");
                        await endpointInstance.Send(cancelCommand).ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
