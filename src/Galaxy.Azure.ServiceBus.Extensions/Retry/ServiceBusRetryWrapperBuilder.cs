using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.Delegates;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public class ServiceBusRetryWrapperBuilder
    {
        private Message _message;
        private IMessageReceiver _messageReceiver;
        private IMessageSender _sender;
        private string _lockToken;
        private int _retryCount;
        private int _interval;
        private Func<Exception, bool> _exceptionPredicate;
        private Execution _execution;
        private OnException _onException;
        private OnScheduling _onScheduling;
        private OnDeadLettering _onDeadLettering;
        private ServiceBusRetryWrapperBuilder()
        {
            _execution = async () => { };
            _onException = async _ => { };
            _onDeadLettering = async _ => { };
            _onScheduling = async (_, __) => { };
            _exceptionPredicate = _ => true;
            _interval = ServiceBusRetryConsts.DEFAULT_INTERVAL;
        }

        public static ServiceBusRetryWrapperBuilder New() => new ServiceBusRetryWrapperBuilder();

        public ServiceBusRetryWrapperBuilder WithMessage(Message message)
        {
            _message = message;
            return this;
        }
        public ServiceBusRetryWrapperBuilder WithReceiver(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
            return this;
        }
        public ServiceBusRetryWrapperBuilder WithSender(IMessageSender sender)
        {
            _sender = sender;
            return this;
        }
        public ServiceBusRetryWrapperBuilder WithLockToken(string lockToken)
        {
            _lockToken = lockToken;
            return this;
        }
        public ServiceBusRetryWrapperBuilder WithRetryCount(int retryCount)
        {
            _retryCount = retryCount;
            return this;
        }
        public ServiceBusRetryWrapperBuilder WithInterval(int interval)
        {
            _interval = interval;
            return this;
        }
        public ServiceBusRetryWrapperBuilder With(OnException onException)
        {
            _onException = onException;
            return this;
        }
        public ServiceBusRetryWrapperBuilder With(OnScheduling onScheduling)
        {
            _onScheduling = onScheduling;
            return this;
        }
        public ServiceBusRetryWrapperBuilder With(OnDeadLettering onDeadLettering)
        {
            _onDeadLettering = onDeadLettering;
            return this;
        }
        public ServiceBusRetryWrapperBuilder With(Execution execution)
        {
            _execution = execution;
            return this;
        }
        public ServiceBusRetryWrapperBuilder With(Func<Exception, bool> exceptionPredicate)
        {
            _exceptionPredicate = exceptionPredicate;
            return this;
        }

        private void EnsureValidState()
        {
            if (_message == default) { throw new ArgumentNullException(nameof(_message)); }

            if (_messageReceiver == default) { throw new ArgumentNullException(nameof(_messageReceiver)); }

            if (_sender == default) { throw new ArgumentNullException(nameof(_sender)); }

            if (_retryCount < 0) { throw new ArgumentNullException(nameof(_retryCount)); }

            if (_lockToken == default) { throw new ArgumentNullException(nameof(_lockToken)); }

        }

        public ServiceBusRetryWrapper Build()
        {
            EnsureValidState();

            return new ServiceBusRetryWrapper(_message,
                    _messageReceiver,
                    _sender, _lockToken,
                    _retryCount,
                    _interval,
                    _execution,
                    _onException,
                    _onScheduling,
                    _onDeadLettering,
                    _exceptionPredicate);
        }


    }
}
