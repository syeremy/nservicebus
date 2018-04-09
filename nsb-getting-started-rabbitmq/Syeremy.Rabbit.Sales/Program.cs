using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Syeremy.Rabbit.Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Rabbit.Sales";
            
            // -- Change log Level to Debug
            var loggerDefinition = LogManager.Use<DefaultFactory>();
            loggerDefinition.Level(LogLevel.Info);
            // --

            var endpointConfiguration = new EndpointConfiguration("Rabbit.Sales");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();
            
            
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate => { immediate.NumberOfRetries(3); });

            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(2);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(5));
                });

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);

        }
    }
}