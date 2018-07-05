using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryCLM.SDK.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToQueryArray<T>(this IEnumerable<T> e, string name, bool last = true)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (e == null 
                || e.Count() == 0) return string.Empty;

            StringBuilder result = new StringBuilder("?");
            for (int i = 0; i < e.Count(); i++)
            {
                result.Append($"{name}=");
                result.Append(e.ElementAt(i));
                if (i != e.Count() - 1)
                    result.Append("&");
            }
            if (!last) result.Append("&");
            return result.ToString();
        }

        public static string ToIdsQueryArray<T>(this IEnumerable<T> e, bool last = true) => 
            e.ToQueryArray("ids", last);

    }
}
