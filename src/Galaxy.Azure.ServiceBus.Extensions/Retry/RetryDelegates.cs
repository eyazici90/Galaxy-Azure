﻿using System;
using System.Threading.Tasks;

namespace Galaxy.Azure.ServiceBus.Extensions.Retry
{
    public class RetryDelegates
    {
        public delegate Task Execution();
        public delegate Task Catch(Exception ex);
        public delegate Task OnException(Exception ex);
        public delegate Task OnScheduling(Exception ex, DateTimeOffset dateTimeOffset);
        public delegate Task OnDeadLettering(Exception ex);

        public static Func<Task> DoNothing { get; } = async () => await Task.CompletedTask; 
    }
}
