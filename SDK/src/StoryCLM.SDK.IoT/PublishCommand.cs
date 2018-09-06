using Newtonsoft.Json;
using StoryCLM.SDK.IoT.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public class PublishMessage : PublishBase
    {
        public PublishMessage(IoTParameters parameters, Stream data = null) : base(parameters, data) {}

        public override async Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                SetMetadata(request);
                if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(token);
                await Task.Run(() =>
                {
                    request.Content = new StreamContent(Data);
                }).ConfigureAwait(false);
            }
        }

        public override async Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(token);
                if (response.StatusCode == HttpStatusCode.NoContent) return;
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.Created)
                    throw new HttpCommandException((int)response.StatusCode, content);

                await Task.Run(() =>
                {
                    Result = JsonConvert.DeserializeObject<Message>(content);
                }).ConfigureAwait(false);
            }
        }
    }
}
