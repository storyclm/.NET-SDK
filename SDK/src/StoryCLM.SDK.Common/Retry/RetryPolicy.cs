using System;

namespace SroryCLM.SDK.Common.Retry
{
    public class RetryPolicy : IRetryPolicy
    {
        public virtual int RetriesCount { get; set; } = 10;

        public virtual TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(50);

        public virtual bool ExponentialBackoff { get; set; } = true;

        public virtual Exception LastException { get; set; }

        public virtual Predicate<Exception> Predicate { get; set; }
    }
}
