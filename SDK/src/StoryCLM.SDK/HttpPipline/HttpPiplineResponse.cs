using System.Net.Http;

namespace StoryCLM.SDK
{
    public class HttpPiplineResponse : HttpPiplineContext
    {
        public HttpResponseMessage Response { get; set; }
    }
}
