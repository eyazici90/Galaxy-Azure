using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;
using Xunit;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceCollectionExtensionsTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;
        public ServiceCollectionExtensionsTests(ServiceProviderFixture serviceProviderFixture) =>
            _serviceProviderFixture = serviceProviderFixture;


        [Theory]
        [InlineData(typeof(IServiceBusPolicy))]
        [InlineData(typeof(IServiceBusRetryHandler))]
        [InlineData(typeof(ServiceBusRetryWrapperBuilder))]
        public void Should_be_registered(Type type)
        {
            var result = _serviceProviderFixture.ServiceProvider.GetService(type);

            result.Should().NotBeNull();
        }
    }
}
