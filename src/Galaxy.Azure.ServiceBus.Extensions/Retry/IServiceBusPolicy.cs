using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Threading.Tasks;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public interface IServiceBusPolicy
    {
        Task ExecuteAsync(Message message,
              IMessageReceiver messageReceiver,
              IMessageSender messageSender,
              string lockToken,
              Func<Task> execution);
    }
}
