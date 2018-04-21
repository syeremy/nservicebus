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
                log.Info("Press '1' to place an order by a Gold Insider client");
                log.Info("Press '2' to place an order by a Gold Standard client");
                log.Info("Press '3' to place an order by a Silver client");
                log.Info("Press '4' to cancel an order by a Gold Insider client");
                log.Info("Press '5' to cancel an order by a Gold Standard client");
                log.Info("Press '6' to cancel an order by a Silver client");
                log.Info("Press 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "SuperImportantClientLtd"
                        };
                        log.Info($"Sending Gold Insider PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command).ConfigureAwait(false);
                        break;

                    case ConsoleKey.D2:
                        var command2 = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "AnotherSuperImportantClientLtd"
                        };
                        log.Info($"Sending Gold Standard PlaceOrder command, OrderId = {command2.OrderId}");
                        await endpointInstance.Send(command2).ConfigureAwait(false);
                        break;

                    case ConsoleKey.D3:
                        var command3 = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "NotSoImportantClient"
                        };
                        log.Info($"Sending Silver PlaceOrder command, OrderId = {command3.OrderId}");
                        await endpointInstance.Send(command3).ConfigureAwait(false);
                        break;

                    case ConsoleKey.D4:
                        var command4 = new CancelOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "SuperImportantClientLtd"
                        };
                        log.Info($"Sending Gold Insider CancelOrder command, OrderId = {command4.OrderId}");
                        await endpointInstance.Send(command4).ConfigureAwait(false);
                        break;

                    case ConsoleKey.D5:
                        var command5 = new CancelOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "AnotherSuperImportantClientLtd"
                        };
                        log.Info($"Sending Gold Standard CancelOrder command, OrderId = {command5.OrderId}");
                        await endpointInstance.Send(command5).ConfigureAwait(false);
                        break;

                    case ConsoleKey.D6:
                        var command6 = new CancelOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ClientId = "NotSoImportantClient"
                        };
                        log.Info($"Sending Silver PlaceOrder command, OrderId = {command6.OrderId}");
                        await endpointInstance.Send(command6).ConfigureAwait(false);
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