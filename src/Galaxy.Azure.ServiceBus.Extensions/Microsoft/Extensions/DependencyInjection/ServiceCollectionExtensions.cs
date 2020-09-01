using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;

namespace  Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBusRetryPolicy(this IServiceCollection services,
            Action<ServiceBusRetryWrapperBuilder> builder)
        {
            services.AddSingleton<IServiceBusRetryHandler>(_ => ServiceBusRetryHandler.Instance);

            var serviceBusBuilder = ServiceBusRetryWrapperBuilder.New();

            builder(serviceBusBuilder);

            services.AddSingleton(serviceBusBuilder);

            services.AddSingleton<IServiceBusPolicy, ServiceBusPolicy>();

            return services;
        }

        public static IServiceCollection AddServiceBusRetryPolicy(this IServiceCollection services,
         Action<IServiceProvider, ServiceBusRetryWrapperBuilder> builder)
        {
            services.AddSingleton<IServiceBusRetryHandler>(_ => ServiceBusRetryHandler.Instance);

            services.AddSingleton(p =>
            {
                var serviceBusBuilder = ServiceBusRetryWrapperBuilder.New();

                builder(p, serviceBusBuilder);

                return serviceBusBuilder;
            });

            services.AddSingleton<IServiceBusPolicy, ServiceBusPolicy>();

            return services;
        }
    }
}
