using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StoryCLM.SDK
{
    public class HttpPiplineRequest : HttpPiplineContext
    {
        public HttpRequestMessage Request { get; set; }
    }
}
