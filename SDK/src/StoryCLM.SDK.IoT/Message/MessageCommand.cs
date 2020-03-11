using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public class MessageCommand<T> : IotHttpCommand where T : class
    {
        public MessageCommand(IoTParameters parameters)
            : base(parameters)
        {
            Method = "GET";
        }

        public T Result { get; set; }

        public override async Task OnExecuting(HttpRequestMessage request, CancellationToken token) => await Task.CompletedTask;

        public override async Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(token);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Result = null;
                    return;
                }
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpCommandException((int)response.StatusCode, content);

                await Task.Run(() => Result = JsonConvert.DeserializeObject<T>(content)).ConfigureAwait(false);
            }
        }
    }
}
