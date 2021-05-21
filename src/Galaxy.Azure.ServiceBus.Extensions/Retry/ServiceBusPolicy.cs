using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Threading.Tasks;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.Delegates;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public partial class ServiceBusPolicy : IServiceBusPolicy
    {
        private readonly IServiceBusRetryHandler _serviceBusRetryHandler;
        private readonly ConfigureBuilder _configureBuilder;
        public ServiceBusPolicy(IServiceBusRetryHandler serviceBusRetryHandler,
            ConfigureBuilder configureBuilder) => (_serviceBusRetryHandler, _configureBuilder) = (serviceBusRetryHandler, configureBuilder);

        public async Task ExecuteAsync(Message message,
            IMessageReceiver messageReceiver,
            IMessageSender messageSender,
            string lockToken,
            Func<Task> execution)
        {
            var builder = ServiceBusRetryWrapperBuilder
                .New()
                .WithMessage(message)
                .WithReceiver(messageReceiver)
                .WithSender(messageSender)
                .WithLockToken(lockToken)
                .With(execution: new Execution(execution));

            _configureBuilder(builder);

            var wrapper = builder.Build();

            await _serviceBusRetryHandler.Handle(wrapper).ConfigureAwait(false);
        }

    }
}
