using FluentAssertions;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using Xunit;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceBusRetryHandlerTests
    {
        [Fact]
        public void Should_be_the_same_when_instance_initialized()
        {
            var firstInstance = ServiceBusRetryHandler.Instance;

            var secondInstance = ServiceBusRetryHandler.Instance;

            firstInstance.Should().Be(secondInstance);
        }
    }
}
