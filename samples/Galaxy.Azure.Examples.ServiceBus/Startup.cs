using Galaxy.Azure.ServiceBus.Extensions.Retry;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Galaxy.Azure.Examples.ServiceBus.Startup))]

namespace Galaxy.Azure.Examples.ServiceBus
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder) =>
            ConfigureServices(builder.Services);

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBusRetryPolicy(builder => builder
               .WithRetryCount(2)
               .WithInterval(20)
               .OnException(async _ =>
               {
                   // onEx
               })
               .OnScheduling(async (_, __) =>
               {
                   // onScheduling
               })
               .OnDeadLettering(async _ =>
               {
                   // onDeadlettering
               }));
        }
    }
}

