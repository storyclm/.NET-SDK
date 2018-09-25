using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT.Models
{
    public class FeedCommand : IotHttpCommand
    {
        public FeedCommand(IoTParameters parameters) 
            : base(parameters)
        {
            Method = "GET";
        }

        public string ContinuationToken { get; set; }

        public int PageSize { get; set; } = 100;

        public IEnumerable<Message> Messages { get; set; }

        public override async Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {

        }

        public override async Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(token);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Messages = Enumerable.Empty<Message>();
                    ContinuationToken = null;
                }
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpCommandException((int)response.StatusCode, content);

                IEnumerable<string> headers;
                if (response.Headers.TryGetValues("Cursor-Position", out headers))
                    ContinuationToken = headers.FirstOrDefault();
                else
                    ContinuationToken = null;

                await Task.Run(() => 
                Messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(content)).ConfigureAwait(false);
            }
        }
    }
}
