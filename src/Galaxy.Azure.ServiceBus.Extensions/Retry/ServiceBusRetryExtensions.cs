using System;
using System.Threading.Tasks;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public static class ServiceBusRetryExtensions
    {
        public static ServiceBusRetryWrapperBuilder OnException(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
              Func<Exception, Task> exFunc) =>
            serviceBusRetryWrapperBuilder.With(onException: new RetryDelegates.OnException(exFunc));

        public static ServiceBusRetryWrapperBuilder OnScheduling(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
              Func<Exception, DateTimeOffset, Task> schedulingFunc) =>
            serviceBusRetryWrapperBuilder.With(onScheduling: new RetryDelegates.OnScheduling(schedulingFunc));

        public static ServiceBusRetryWrapperBuilder OnDeadLettering(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
              Func<Exception, Task> deadletteringFunc) =>
            serviceBusRetryWrapperBuilder.With(onDeadLettering: new RetryDelegates.OnDeadLettering(deadletteringFunc));

        public static ServiceBusRetryWrapperBuilder RetryCount(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
            int retryCount) =>
            serviceBusRetryWrapperBuilder.WithRetryCount(retryCount: retryCount);

        public static ServiceBusRetryWrapperBuilder RetryCount(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
            int retryCount,
            int interval) =>
            serviceBusRetryWrapperBuilder.WithRetryCount(retryCount: retryCount)
                                         .WithInterval(interval);

        public static async Task ExecuteAsync(this ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder,
            Func<Task> execution)
        {
            var handler = ServiceBusRetryHandler.Instance;

            serviceBusRetryWrapperBuilder.With(execution: new RetryDelegates.Execution(execution));

            var wrapper = serviceBusRetryWrapperBuilder.Build();

            await handler.Handle(wrapper).ConfigureAwait(false);
        }
    }
}
