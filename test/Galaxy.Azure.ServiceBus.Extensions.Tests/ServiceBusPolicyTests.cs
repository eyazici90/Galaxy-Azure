using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceBusPolicyTests : IClassFixture<ServiceBusRetryWrapperBuilderFixture>,
         IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceBusRetryWrapperBuilderFixture _fixture;
        private readonly ServiceProviderFixture _serviceProviderFixture;
        public ServiceBusPolicyTests(ServiceBusRetryWrapperBuilderFixture fixture,
            ServiceProviderFixture serviceProviderFixture)
        {
            _fixture = fixture;
            _serviceProviderFixture = serviceProviderFixture;
        }

        [Fact]
        public async Task Should_handle_execution()
        {
            var exCount = 0;

            await ServiceBusPolicy.Apply()
                .WithMessage(_fixture.Msg)
                .WithSender(_fixture.MessageSender)
                .WithReceiver(_fixture.MessageReceiver)
                .WithLockToken(_fixture.LockToken)
                .RetryCount(2)
                .OnDeadLettering(async _ => { })
                .OnException(async _ => exCount++)
                .OnScheduling(async (_, __) => { })
                .ExecuteAsync(async () => await Delegates.DoNothing());

            exCount.Should().Be(0);
        }

        [Fact]
        public void Should_not_handle_execution()
        {
            Func<Task> act = async () => await ServiceBusPolicy.Apply(ex => ex is ArgumentOutOfRangeException)
                  .WithMessage(_fixture.Msg)
                  .WithSender(_fixture.MessageSender)
                  .WithReceiver(_fixture.MessageReceiver)
                  .WithLockToken(_fixture.LockToken)
                  .RetryCount(2)
                  .OnException(async _ => { })
                  .ExecuteAsync(async () => throw new ArgumentNullException());

            act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public async Task Should_be_scheduled_when_executied()
        {
            DateTimeOffset result = DateTimeOffset.Now;

            await ServiceBusPolicy.Apply()
                .WithMessage(_fixture.Msg)
                .WithSender(_fixture.MessageSender)
                .WithReceiver(_fixture.MessageReceiver)
                .WithLockToken(_fixture.LockToken)
                .RetryCount(2)
                .OnScheduling(async (ex, scheduledTime) => result = scheduledTime)
                .ExecuteAsync(async () => throw new Exception());

            result.Second.Should().Be(DateTimeOffset.Now.AddSeconds(5 * 1).Second);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public async Task Should_be_scheduled_by_interval_when_executied(int interval)
        {
            DateTimeOffset result = DateTimeOffset.Now;

            await ServiceBusPolicy.Apply()
                .WithMessage(_fixture.Msg)
                .WithSender(_fixture.MessageSender)
                .WithReceiver(_fixture.MessageReceiver)
                .WithLockToken(_fixture.LockToken)
                .RetryCount(2, interval)
                .OnScheduling(async (ex, scheduledTime) => result = scheduledTime)
                .ExecuteAsync(async () => throw new Exception());

            result.Second.Should().Be(DateTimeOffset.Now.AddSeconds(interval * 1).Second);
        }

        [Fact]
        public async Task Should_be_dead_lettered_when_total_reached_total_count()
        {
            var result = false;

            await ServiceBusPolicy.Apply()
                .WithMessage(_fixture.Msg)
                .WithSender(_fixture.MessageSender)
                .WithReceiver(_fixture.MessageReceiver)
                .WithLockToken(_fixture.LockToken)
                .RetryCount(0)
                .OnDeadLettering(async _ => result = true)
                .ExecuteAsync(async () => throw new Exception());

            result.Should().BeTrue();

        }

        [Fact]
        public async Task Should_be_handled_by_policy_globally()
        {
            var policy = _serviceProviderFixture.The<IServiceBusPolicy>();

            var executedSuccesfully = false;
            await policy.ExecuteAsync(_fixture.Msg,
                              _fixture.MessageReceiver,
                              _fixture.MessageSender,
                              _fixture.LockToken,
                              async () =>
                              {
                                  executedSuccesfully = true;
                              });

            executedSuccesfully.Should().BeTrue();
        }



    }
}
