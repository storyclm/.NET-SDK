using System.Collections.Generic;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public class CLMAnalitycsFeedPage<T>
    {
        public long? ContinuationToken { get; set; }

        public IEnumerable<T> Result { get; set; }
    }
}
