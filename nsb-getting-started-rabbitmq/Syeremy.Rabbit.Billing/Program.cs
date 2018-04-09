using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Syeremy.Rabbit.Billing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Rabbit.Billing";

            var endpointConfiguration = new EndpointConfiguration("Rabbit.Billing");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}