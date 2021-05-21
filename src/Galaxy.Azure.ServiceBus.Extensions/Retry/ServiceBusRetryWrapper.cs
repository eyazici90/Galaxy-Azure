using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.Delegates;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public class ServiceBusRetryWrapper
    {
        public Message Message { get; }
        public IMessageReceiver MessageReceiver { get; }
        public IMessageSender Sender { get; }
        public string LockToken { get; }
        public int RetryCount { get; }
        public int Interval { get; }
        public Execution Execution { get; }
        public OnException OnException { get; }
        public OnScheduling OnScheduling { get; }
        public OnDeadLettering OnDeadLettering { get; }
        public Func<Exception, bool> ExceptionPredicate { get; }
        public ServiceBusRetryWrapper(Message message,
            IMessageReceiver messageReceiver,
            IMessageSender sender,
            string lockToken,
            int retryCount,
            int interval,
            Execution execution,
            OnException onException,
            OnScheduling onScheduling,
            OnDeadLettering onDeadLettering,
            Func<Exception, bool> exceptionPredicate)
        {
            Message = message;
            MessageReceiver = messageReceiver;
            Sender = sender;
            LockToken = lockToken;
            RetryCount = retryCount;
            Interval = interval;
            Execution = execution;
            OnException = onException;
            OnScheduling = onScheduling;
            OnDeadLettering = onDeadLettering;
            ExceptionPredicate = exceptionPredicate;
        }
    }
}
