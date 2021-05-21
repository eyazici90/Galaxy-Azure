using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Moq;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;
using Xunit;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceBusRetryWrapperBuilderTests
    {

        [Fact]
        public void Should_set_the_execution_when_called()
        {
            var sut = _sut();

            Delegates.Execution ex = async () => await Delegates.DoNothing();

            sut.With(execution: ex);

            var result = sut.Build();

            result.Execution.Should().Be(ex);
        }

        [Fact]
        public void Should_set_the_onException_when_called()
        {
            var sut = _sut();

            Delegates.OnException ex = async _ => await Delegates.DoNothing();

            sut.With(onException: ex);

            var result = sut.Build();

            result.OnException.Should().Be(ex);
        }

        [Fact]
        public void Should_set_the_onScheduled_when_called()
        {
            var sut = _sut();

            Delegates.OnScheduling ex = async (_, __) => await Delegates.DoNothing();

            sut.With(onScheduling: ex);

            var result = sut.Build();

            result.OnScheduling.Should().Be(ex);
        }

        [Fact]
        public void Should_set_the_onDeathlettering_when_called()
        {
            var sut = _sut();

            Delegates.OnDeadLettering ex = async _ => await Delegates.DoNothing();

            sut.With(onDeadLettering: ex);

            var result = sut.Build();

            result.OnDeadLettering.Should().Be(ex);
        }

        [Fact]
        public void Should_set_the_message_when_called()
        {
            var sut = _sut();

            var msg = new Message();

            sut.WithMessage(msg);

            var result = sut.Build();

            result.Message.Should().Be(msg);
        }

        [Theory]
        [InlineData(nameof(ServiceBusRetryWrapper.Execution))]
        [InlineData(nameof(ServiceBusRetryWrapper.OnException))]
        [InlineData(nameof(ServiceBusRetryWrapper.OnScheduling))]
        [InlineData(nameof(ServiceBusRetryWrapper.OnDeadLettering))]
        public void Should_not_be_null(string paramName)
        {
            var sut = _sut();

            var wrapper = sut.Build();

            var result = wrapper.GetType()
                .GetProperty(paramName)
                .GetValue(wrapper);

            result.Should().NotBeNull();
        }

        private Func<ServiceBusRetryWrapperBuilder> _sut = () =>
        {
            var fakeMsg = Mock.Of<Message>();
            var fakeSender = Mock.Of<IMessageSender>();
            var fakeReceiver = Mock.Of<IMessageReceiver>();
            var fakeLockToken = Guid.NewGuid().ToString();
            var fakeRetryCount = 5;

            return ServiceBusRetryWrapperBuilder.New()
                    .WithReceiver(fakeReceiver)
                    .WithSender(fakeSender)
                    .WithMessage(fakeMsg)
                    .WithRetryCount(fakeRetryCount)
                    .WithLockToken(fakeLockToken);
        };

    }
}
