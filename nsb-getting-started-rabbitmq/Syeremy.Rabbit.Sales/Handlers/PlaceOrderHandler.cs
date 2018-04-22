using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Logging;
using Syeremy.Messages.Commands;
using Syeremy.Messages.Events;

namespace Syeremy.Rabbit.Sales.Handlers
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        private IConfiguration _configuration;

        public PlaceOrderHandler()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            _configuration = builder.Build();
        }
        
        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            LogTrace(message);
            // This is normally where some business logic would occur
            throw new Exception("An exception occurred in the handler.");
            
            
            var orderPlaced = new OrderPlaced
            {
                OrderId =  message.OrderId,
                ClientId = message.ClientId
            };
            
            var publishOptions = new PublishOptions();
            publishOptions.SetHeader("insider", GetInsiderProgramValue(message));
            publishOptions.SetHeader("membership", GetMembership(message));

            // -- Echange : amq.topic, Queue : Syeremy-Messages-Events-OrderPlaced -- binding needed.
            await context.Publish(orderPlaced, publishOptions);

        }
        
        private string GetInsiderProgramValue(PlaceOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ? "1" : "0";
        }

        private string GetMembership(PlaceOrder cancelOrder)
        {
            // get some data from a database or something

            return cancelOrder.ClientId.Equals("SuperImportantClientLtd") ||
                   cancelOrder.ClientId.Equals("AnotherSuperImportantClientLtd")
                ? "gold"
                : "silver";
        }
        
        private void LogTrace(PlaceOrder message)
        {
            using (var sqlConn = new SqlConnection(_configuration.GetConnectionString("Nsb-Rabbitmq-Recoverability")))
            {
                sqlConn.Open();
                using (var command = sqlConn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [dbo].[RetriesTrace]
                        ([OrderId]
                        ,[AppEntryTime]
                        ,[DbEntryTime])
                    VALUES
                        (@OrderId
                        ,@TimeNow
                        ,@TimeNow)";

                    command.Parameters.Add("@OrderId", SqlDbType.VarChar, 200).Value = message.OrderId;
                    command.Parameters.Add("@TimeNow", SqlDbType.DateTime).Value = DateTime.Now;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}