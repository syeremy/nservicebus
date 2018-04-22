using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

namespace Syeremy.Rabbit.Sales
{
    class Program
    {
        private static IConfiguration _configuration;

        
        static async Task Main(string[] args)
        {
            Init();
            
            Console.Title = "Rabbit.Sales";
            
            // -- Change log Level to Debug
            var loggerDefinition = LogManager.Use<DefaultFactory>();
            loggerDefinition.Level(LogLevel.Info);
            // --

            var endpointConfiguration = new EndpointConfiguration("Rabbit.Sales");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();
            
            
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate => { immediate.NumberOfRetries(0); });

            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(20);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(1));
                });
            
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(() => new SqlConnection(_configuration.GetConnectionString("Nsb-Rabbitmq-Recoverability")));

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Any key to Exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);

        }

        static void Init()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            _configuration = builder.Build();
        }
    }
}