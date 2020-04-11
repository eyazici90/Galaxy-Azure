using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Threading.Tasks;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public partial class ServiceBusPolicy : IServiceBusPolicy
    {
        private readonly IServiceBusRetryHandler _serviceBusRetryHandler;
        private readonly ServiceBusRetryWrapperBuilder _serviceBusRetryWrapperBuilder;
        public ServiceBusPolicy(IServiceBusRetryHandler serviceBusRetryHandler,
            ServiceBusRetryWrapperBuilder serviceBusRetryWrapperBuilder)
        {
            _serviceBusRetryHandler = serviceBusRetryHandler;
            _serviceBusRetryWrapperBuilder = serviceBusRetryWrapperBuilder;
        }


        public async Task ExecuteAsync(Message message,
            IMessageReceiver messageReceiver,
            IMessageSender messageSender,
            string lockToken,
            Func<Task> execution)
        {
            _serviceBusRetryWrapperBuilder
                .WithMessage(message)
                .WithReceiver(messageReceiver)
                .WithSender(messageSender)
                .WithLockToken(lockToken)
                .With(execution: new RetryDelegates.Execution(execution));

            var wrapper = _serviceBusRetryWrapperBuilder.Build();

            await _serviceBusRetryHandler.Handle(wrapper).ConfigureAwait(false);
        }

    }
}
