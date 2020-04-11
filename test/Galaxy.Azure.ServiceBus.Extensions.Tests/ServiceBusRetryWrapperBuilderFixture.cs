using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Moq;
using System;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ServiceBusRetryWrapperBuilderFixture
    {
        public Message Msg
        {
            get
            {
                var val = new Message();
                val.SystemProperties.GetType()
                  .GetProperty("SequenceNumber")
                  .SetValue(val.SystemProperties, 123);
                return val;
            }
        }

        public IMessageSender MessageSender => Mock.Of<IMessageSender>();
        public IMessageReceiver MessageReceiver => Mock.Of<IMessageReceiver>();
        public string LockToken => Guid.NewGuid().ToString();
    }
}
