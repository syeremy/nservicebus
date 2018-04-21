using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Syeremy.Rabbit.GoldInsiderClientServices
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            Console.Title = "Rabbit.GoldInsiderClientServices";

            var endpointConfiguration = new EndpointConfiguration("Rabbit.GoldInsiderClientServices");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseRoutingTopology(topologyFactory:createDurableExchangesAndQueues => new CustomRoutingTopology(createDurableExchangesAndQueues));


            
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}