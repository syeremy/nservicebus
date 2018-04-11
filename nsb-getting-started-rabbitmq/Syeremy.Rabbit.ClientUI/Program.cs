using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Commands;

namespace Syeremy.Rabbit.ClientUI
{
    class Program
    {
        static readonly ILog log = LogManager.GetLogger<Program>();

        
        static async Task Main(string[] args)
        {
            Console.Title = "Rabbit.ClientUI";
            
            // -- Change log Level to Debug
            var loggerDefinition = LogManager.Use<DefaultFactory>();
            loggerDefinition.Level(LogLevel.Info);
            // --

            var endpointConfiguration = new EndpointConfiguration("Rabbit.ClientUI");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();
            
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Rabbit.Sales");
            routing.RouteToEndpoint(typeof(CancelOrder), "Rabbit.Sales");
            
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await RunLoop(endpointInstance).ConfigureAwait(false);
            await endpointInstance.Stop().ConfigureAwait(false);
        }
        
        

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place an order, Press 'C' cancel an order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var placeOrderCommand = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        log.Info($"Sending PlaceOrder command, OrderId = {placeOrderCommand.OrderId}");
                        //-- Echange : (AMQP default), Queue : Rabbit.Sales -- No binding needed.
                        await endpointInstance.Send(placeOrderCommand).ConfigureAwait(false);

                        break;
                    case ConsoleKey.C:
                        // Instantiate the command
                        var cancelOrderCommand = new CancelOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        log.Info($"Sending CancelOrder command, OrderId = {cancelOrderCommand.OrderId}");
                        await endpointInstance.Send(cancelOrderCommand).ConfigureAwait(false);

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