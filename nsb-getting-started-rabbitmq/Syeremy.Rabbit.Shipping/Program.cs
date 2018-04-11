using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Syeremy.Rabbit.Shipping
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Rabbit.Shipping";

            var endpointConfiguration = new EndpointConfiguration("Rabbit.Shipping");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}