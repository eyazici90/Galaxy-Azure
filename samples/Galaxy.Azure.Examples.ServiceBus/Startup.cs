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
               .With(onException: async _ =>
               {
                   // onEx
               })
               .With(onScheduling: async (_, __) =>
               {
                   // onScheduling
               })
               .With(onDeadLettering: async _ =>
               {
                   // onDeadlettering
               }));
        }
    }
}

