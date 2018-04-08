using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Syeremy.Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
            
        }
    }
}
