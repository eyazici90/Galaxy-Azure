using System;
using System.Threading.Tasks;
using static Galaxy.Azure.ServiceBus.Extensions.Retry.Delegates;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public static class Executor
    {
        public static async Task Execute<TException>(Execution execution,
            Catch @catch) where TException : Exception =>
            await Execute<TException>(execution, @catch, _ => true).ConfigureAwait(false);


        public static async Task Execute<TException>(Execution execution,
            Catch @catch,
            Func<Exception, bool> when) where TException : Exception
        {
            try
            {
                await execution().ConfigureAwait(false);
            }
            catch (TException ex) when (when(ex))
            {
                await @catch(ex).ConfigureAwait(false);
            }
        }
    }
}
