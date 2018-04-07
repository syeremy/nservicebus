using System;
using System.IO.Compression;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Logging;

namespace Syeremy.Order
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "El Cavernas";
            LogManager.Use<DefaultFactory>().Level(LogLevel.Info);

            try
            {
                var endopointConfiguration = new EndpointConfiguration("elcavernas.queue.orders");
                var transport = endopointConfiguration.UseTransport<RabbitMQTransport>();
                transport.ConnectionString("host=localhost;username=guest;password=guest");
                transport.UseConventionalRoutingTopology();
                transport.UseDurableExchangesAndQueues(true);
            
                endopointConfiguration.UsePersistence<InMemoryPersistence>();
                endopointConfiguration.SendFailedMessagesTo("elcavernas.error");

                var endpointInstance = await Endpoint.Start(endopointConfiguration).ConfigureAwait(false);

                try
                {
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                }
                finally
                {
                    await endpointInstance.Stop().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
