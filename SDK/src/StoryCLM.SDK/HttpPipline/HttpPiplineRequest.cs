using System.Net.Http;

namespace StoryCLM.SDK
{
    public class HttpPiplineRequest : HttpPiplineContext
    {
        public HttpRequestMessage Request { get; set; }
    }
}
