using System; 

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public partial class ServiceBusPolicy
    {
        public static ServiceBusRetryWrapperBuilder Apply() =>
               Apply<Exception>();

        public static ServiceBusRetryWrapperBuilder Apply(Func<Exception, bool> when) =>
            Apply<Exception>(when);

        private static ServiceBusRetryWrapperBuilder Apply<TException>() where TException : Exception =>
            Apply<TException>(_ => true);

        private static ServiceBusRetryWrapperBuilder Apply<TException>(Func<Exception, bool> when) where TException : Exception =>
           ServiceBusRetryWrapperBuilder.New()
                                        .With(when);
    }
}
