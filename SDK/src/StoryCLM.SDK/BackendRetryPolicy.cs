using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK
{
    public class BackendRetryPolicy : IRetryPolicy
    {
        public virtual int RetriesCount { get; set; } = 15;

        public virtual TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(50);

        public virtual bool ExponentialBackoff { get; set; } = true;

        public virtual Exception LastException { get; set; }

        public virtual Predicate<Exception> Predicate { get; set; }
    }
}
