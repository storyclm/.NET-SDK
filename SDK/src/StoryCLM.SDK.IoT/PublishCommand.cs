using Newtonsoft.Json;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public class PublishMessage : ResultCommand<Message>
    {
        public PublishMessage(IoTParameters parameters, Stream data = null) : base(parameters, data) { }

        const string META_PREFIX = "s-meta-";

        IDictionary<string, string> _metadata = new Dictionary<string, string>();

        public string this[string item]
        {
            get => _metadata[item];
            set
            {
                _metadata[item] = value;
            }
        }

        protected void SetMetadata(HttpRequestMessage request)
        {
            foreach (var t in _metadata)
                request.Headers.Add($"{META_PREFIX}{t.Key}", t.Value);
        }

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
