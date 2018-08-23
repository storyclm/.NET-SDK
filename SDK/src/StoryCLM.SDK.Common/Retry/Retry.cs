using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SroryCLM.SDK.Common.Retry
{
    public class Retry : IRetry
    {
        public async Task Execute(Func<Task> action,
            IRetryPolicy retryPolicy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                bool retry = retryPolicy == null ? false : retryPolicy.RetriesCount > 0;
                TimeSpan retryInterval = retryPolicy?.Interval ?? TimeSpan.Zero;
                int retryCount = 0;
                do
                {
                    try
                    {
                        await action();
                        return;
                    }
                    catch (Exception ex)
                    {
                        //обрабатываем повтор.
                        if (!retry
                            || retryPolicy == null
                            || retryCount >= retryPolicy.RetriesCount) throw;

                        //необходим ли повтор (повтор по условию).
                        if (retryPolicy.Predicate != null)
                            if (!retryPolicy.Predicate(ex)) throw;

                        //задержка между повторами
                        if (retryInterval > TimeSpan.Zero) // без задержки или с задержкой
                        {
                            if (retryPolicy.ExponentialBackoff) // экспоненциальная задержка
                            {
                                var pow = Math.Pow(2, retryCount);
                                int delay = (int)(retryInterval.TotalMilliseconds * (pow - 1) / 2);
                                await Task.Delay(delay, cancellationTokenSource.Token).ConfigureAwait(false);
                            }
                            else // линейная задержка
                                await Task.Delay((int)retryInterval.TotalMilliseconds * retryCount, cancellationTokenSource.Token).ConfigureAwait(false); 
                        }

                        retryCount++;
                    }
                }
                while (retry);
            }
        }
    }
}
