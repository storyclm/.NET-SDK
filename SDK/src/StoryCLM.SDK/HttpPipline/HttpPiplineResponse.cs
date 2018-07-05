using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StoryCLM.SDK
{
    public class HttpPiplineResponse : HttpPiplineContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}
