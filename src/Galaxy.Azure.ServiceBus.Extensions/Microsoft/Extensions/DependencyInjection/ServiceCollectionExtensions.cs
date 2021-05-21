using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.Delegates;

namespace  Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBusRetryPolicy(this IServiceCollection services,
             ConfigureBuilder builder)
        {
            services.AddSingleton<IServiceBusRetryHandler>(_ => ServiceBusRetryHandler.Instance);

            services.AddSingleton<IServiceBusPolicy, ServiceBusPolicy>();
            services.AddTransient(_ => builder);
            return services;
        }

        public static IServiceCollection AddServiceBusRetryPolicy(this IServiceCollection services,
            Action<IServiceProvider, ServiceBusRetryWrapperBuilder> builderFunc)
        {
            services.AddSingleton<IServiceBusRetryHandler>(_ => ServiceBusRetryHandler.Instance);

            services.AddTransient(p => new ConfigureBuilder(b => builderFunc(p, b)));
            services.AddSingleton<IServiceBusPolicy, ServiceBusPolicy>();

            return services;
        }

    }
}
