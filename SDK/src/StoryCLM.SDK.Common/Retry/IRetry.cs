using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SroryCLM.SDK.Common.Retry
{
    public interface IRetry
    {
        Task Execute(Func<Task> action, IRetryPolicy retryPolicy, CancellationToken cancellationToken = default(CancellationToken));
    }
}
