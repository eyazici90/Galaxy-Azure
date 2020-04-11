
# Galaxy-Azure
C# library for useful extensions on Azure Services. 

##  Features

 - Fluent usage retry policy for que triggered Azure functions.
 - Dependency injection support for globally configurated policy.
 - v3 Azure Functions supported.

 ##  Samples
 

 - [Galaxy.Azure.Examples.ServiceBus](https://github.com/eyazici90/Galaxy-Azure/tree/master/samples/Galaxy.Azure.Examples.ServiceBus)

## Usage

        [FunctionName("FluentRetry")]
        public static async Task Run(
            [ServiceBusTrigger(".", ".", Connection = "ServiceBusConnectionString")]Message message,
            MessageReceiver messageReceiver, 
            string lockToken,
            [ServiceBus(".", EntityType.Topic, Connection = "ServiceBusConnectionString")] MessageSender sender,
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

***With DI***

       private void ConfigureServices(IServiceCollection services)
            {
                services.AddServiceBusRetryPolicy(builder => builder
                   .WithRetryCount(2)
                   .WithInterval(20)
                   .OnException(async _ =>
                   {
                       // onEx
                   })
                   .OnScheduling(async (_, __) =>
                   {
                       // onScheduling
                   })
                   .OnDeadLettering(async _ =>
                   {
                       // onDeadlettering
                   }));
            }
## 
    public class ExponentialRetryDI
        {
            private readonly IServiceBusPolicy _serviceBusPolicy;
            public ExponentialRetryDI(IServiceBusPolicy serviceBusPolicy)
            {
                _serviceBusPolicy = serviceBusPolicy;
            }
    
            [FunctionName("ExponentialRetryDI")]
            public async Task Run(
                [ServiceBusTrigger(".", ".", Connection = "ServiceBusConnectionString")]Message message,
                MessageReceiver messageReceiver,
                string lockToken,
                [ServiceBus(".", EntityType.Topic, Connection = "ServiceBusConnectionString")] MessageSender sender,
                ILogger log) =>
               await _serviceBusPolicy.ExecuteAsync(message,
                   messageReceiver,
                   sender,
                   lockToken,
                   async () =>
                   {
                       // ur code 
                   });
        }
