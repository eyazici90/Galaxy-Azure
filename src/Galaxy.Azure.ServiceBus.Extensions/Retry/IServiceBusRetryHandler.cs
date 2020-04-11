using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public interface IServiceBusRetryHandler
    {
        Task Handle(ServiceBusRetryWrapper serviceBusRetryWrapper);
    }
}
