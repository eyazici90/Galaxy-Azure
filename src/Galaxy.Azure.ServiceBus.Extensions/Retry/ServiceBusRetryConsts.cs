
namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public static class ServiceBusRetryConsts
    {
        public const string RETRY_COUNT = "retry-count";
        public const string SEQUENCENUMBER = "original-SequenceNumber";
        public const string DEAD_LETTER_REASON = "Exhausted all retries";
        public const string SUCCESS_SUBSCRIPTIONS= "success-Subscriptions";
        public const int DEFAULT_INTERVAL = 5;
    }
}
