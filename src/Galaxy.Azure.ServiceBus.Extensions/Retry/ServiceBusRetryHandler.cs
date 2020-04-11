using System;
using System.Threading.Tasks;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.RetryDelegates;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public sealed class ServiceBusRetryHandler : IServiceBusRetryHandler
    {
        public static ServiceBusRetryHandler Instance { get; } = new ServiceBusRetryHandler();
        public async Task Handle(ServiceBusRetryWrapper serviceBusRetryWrapper)
        {
            var serviceBusExecutionFunc = GetExecutionFunc(serviceBusRetryWrapper);

            var serviceBusExceptionFunc = GetExceptionFunc(serviceBusRetryWrapper);

            await Executor.Execute<Exception>(serviceBusExecutionFunc,
                serviceBusExceptionFunc,
                serviceBusRetryWrapper.ExceptionPredicate).ConfigureAwait(false);
        }


        private Execution GetExecutionFunc(ServiceBusRetryWrapper serviceBusRetryWrapper) =>
            async () =>
            {
                await serviceBusRetryWrapper.Execution().ConfigureAwait(false);
                await serviceBusRetryWrapper.MessageReceiver.CompleteAsync(serviceBusRetryWrapper.LockToken).ConfigureAwait(false); 
            };

        private Catch GetExceptionFunc(ServiceBusRetryWrapper serviceBusRetryWrapper) =>
            async (ex) =>
            {
                var message = serviceBusRetryWrapper.Message;
                await serviceBusRetryWrapper.OnException(ex).ConfigureAwait(false);
                if (!message.UserProperties.ContainsKey(ServiceBusRetryConsts.RETRY_COUNT))
                {
                    message.UserProperties[ServiceBusRetryConsts.RETRY_COUNT] = 0;
                    message.UserProperties[ServiceBusRetryConsts.SEQUENCENUMBER] = message.SystemProperties.SequenceNumber;
                }

                if ((int)message.UserProperties[ServiceBusRetryConsts.RETRY_COUNT] < serviceBusRetryWrapper.RetryCount)
                {
                    var retryMessage = message.Clone();
                    var retryCount = (int)message.UserProperties[ServiceBusRetryConsts.RETRY_COUNT] + 1;
                    var interval = serviceBusRetryWrapper.Interval * retryCount;
                    var scheduledTime = DateTimeOffset.Now.AddSeconds(interval);

                    retryMessage.UserProperties[ServiceBusRetryConsts.RETRY_COUNT] = retryCount;
                    await serviceBusRetryWrapper.Sender.ScheduleMessageAsync(retryMessage, scheduledTime);
                    await serviceBusRetryWrapper.MessageReceiver.CompleteAsync(serviceBusRetryWrapper.LockToken);

                    await serviceBusRetryWrapper.OnScheduling(ex, scheduledTime).ConfigureAwait(false);
                }
                else
                {
                    await serviceBusRetryWrapper.OnDeadLettering(ex).ConfigureAwait(false);
                    await serviceBusRetryWrapper.MessageReceiver.DeadLetterAsync(serviceBusRetryWrapper.LockToken, ServiceBusRetryConsts.DEAD_LETTER_REASON);
                }
            };
    }
}
