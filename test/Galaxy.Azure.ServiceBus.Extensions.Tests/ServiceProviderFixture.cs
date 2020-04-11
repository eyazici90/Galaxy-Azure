using Microsoft.Extensions.DependencyInjection;
using System;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; }
        public ServiceProviderFixture()
        {
            var services = new ServiceCollection();

            services.AddServiceBusRetryPolicy(builder => builder
               .WithRetryCount(3)
               .WithInterval(5)
           );

            ServiceProvider = services.BuildServiceProvider();
        }

        public T The<T>() where T : class =>
            ServiceProvider.GetService<T>();
    }
}
