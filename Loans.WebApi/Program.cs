using Messages.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace Loans.WebApi
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
     .Build();
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });
            Log.Information("The application started well...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
  .UseNServiceBus(context =>
  {
      var endpointConfiguration = new EndpointConfiguration(GetQueue("queueName"));
      endpointConfiguration.PurgeOnStartup(true);
      endpointConfiguration.EnableInstallers();
      var outboxSettings = endpointConfiguration.EnableOutbox();
      var recoverability = endpointConfiguration.Recoverability();
      recoverability.Delayed(
          customizations: delayed =>
          {
              delayed.NumberOfRetries(2);
              delayed.TimeIncrease(TimeSpan.FromMinutes(4));
          });
      recoverability.Immediate(
          customizations: immediate =>
          {
              immediate.NumberOfRetries(3);

          });

      var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
      transport.UseConventionalRoutingTopology()
      .ConnectionString(Configuration.GetConnectionString("RabbitMQ"));
      var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
      var connection = Configuration.GetConnectionString("LoansOutbox");
      persistence.SqlDialect<SqlDialect.MsSqlServer>();
      persistence.ConnectionBuilder(
          connectionBuilder: () =>
          {
              return new SqlConnection(connection);
          });

      var routing = transport.Routing();
      routing.RouteToEndpoint(
           messageType: typeof(CheckLoanValid),
           destination: "RulesNSB");
      endpointConfiguration.SendFailedMessagesTo(GetQueue("errorQueue"));
      endpointConfiguration.AuditProcessedMessagesTo(GetQueue("auditQueue"));
      var conventions = endpointConfiguration.Conventions();
      conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
      conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

      string GetQueue(string queueName)
      {
          return Configuration.GetSection("Queues").GetSection(queueName).Value;
      }
      return endpointConfiguration;
  })
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>()
                 .UseConfiguration(Configuration)
                 .UseSerilog();
   });        
    }
}
