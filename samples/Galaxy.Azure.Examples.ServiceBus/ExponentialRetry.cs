using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System.Threading.Tasks;

namespace Galaxy.Azure.Examples.ServiceBus
{
    public static class ExponentialRetry
    {
        [FunctionName("ExponentialRetry")]
        public static async Task Run(
            [ServiceBusTrigger("alteration-integration", "alteration-set-finished", Connection = "ServiceBusConnectionString")]Message message,
            MessageReceiver messageReceiver, 
            string lockToken,
            [ServiceBus("alteration-integration", EntityType.Topic, Connection = "ServiceBusConnectionString")] MessageSender sender,
            ILogger log) =>

            await ServiceBusPolicy.Apply()
                .WithMessage(message)
                .WithSender(sender)
                .WithReceiver(messageReceiver)
                .WithLockToken(lockToken)
                .RetryCount(retryCount: 2, interval: 20)
                .OnDeadLettering(async _ => log.LogWarning("Deadlettering the msg"))
                .OnException(async ex => log.LogError(ex.Message))
                .OnScheduling(async (_, __) => log.LogWarning("Scheduling the msg"))
                .ExecuteAsync(async () =>
                {
                    // ur code
                    throw new System.Exception();
                });
    }
}
