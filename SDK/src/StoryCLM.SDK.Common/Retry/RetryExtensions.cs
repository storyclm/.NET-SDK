using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SroryCLM.SDK.Common.Retry
{
    public static class RetryExtensions
    {
        public async static Task Retry(this Func<Task> action, IRetryPolicy policy, CancellationToken token = default(CancellationToken))
        {
            if (action == null) return;
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            Retry retry = new Retry();
            await retry.Execute(async () => await action(), policy, token).ConfigureAwait(false);
        }

        public async static Task<T> Retry<T>(this Func<Task<T>> action, IRetryPolicy policy, CancellationToken token = default(CancellationToken))
            => await action.Retry(policy, token);
    }
}
