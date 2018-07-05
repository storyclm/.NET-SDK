using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StoryCLM.SDK
{
    public class HttpPiplineContext
    {
        public IHttpCommand Command { get; set;}

        public SCLM Executor { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
