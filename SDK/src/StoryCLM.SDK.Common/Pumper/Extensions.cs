using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Common.Pumper
{
    public static class Extensions
    {
        public static async Task Pump<T>(this IEnumerable<T> source, Func<IEnumerable<T>, Task> handler, int buffer = 100)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (source == null) throw new ArgumentNullException(nameof(source));
            SequentialPumper<T> acc = new SequentialPumper<T>(buffer);
            await acc.Pump(source, handler);
        }
    }
}
