using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Galaxy.Azure.ServiceBus.Extensions.Retry;

namespace Galaxy.Azure.Examples.ServiceBus
{
    public class ExponentialRetryDI
    {
        private readonly IServiceBusPolicy _serviceBusPolicy;
        public ExponentialRetryDI(IServiceBusPolicy serviceBusPolicy)
        {
            _serviceBusPolicy = serviceBusPolicy;
        }

        [FunctionName("ExponentialRetryDI")]
        public async Task Run(
            [ServiceBusTrigger("<your-topic-name>", ".", Connection = "ServiceBusConnectionString")]Message message,
            MessageReceiver messageReceiver,
            string lockToken,
            [ServiceBus("<your-topic-name>", EntityType.Topic, Connection = "ServiceBusConnectionString")] MessageSender sender,
            ILogger log) =>
           await _serviceBusPolicy
               .ExecuteAsync(message,
               messageReceiver,
               sender,
               lockToken,
               async () =>
               {
                   // ur code
                   log.LogWarning("DI func executed succesfully"); 
                 //  throw new System.Exception();
               });
    }
}
