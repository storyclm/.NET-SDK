using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public abstract class BackendCommand : IHttpCommand
    {
        public virtual string Method { get; set; } = "GET";

        public virtual Uri Uri { get; set; }

        public virtual TimeSpan ExpiryTime { get; set; } = TimeSpan.FromMinutes(5);

        public virtual Exception Exception { get; set; }

        public virtual void Dispose() { }

        public abstract Task OnExecuting(HttpRequestMessage request, CancellationToken token);

        public abstract Task OnExecuted(HttpResponseMessage response, CancellationToken token);
    }

    public class BackendCommand<TResult> : BackendCommand where TResult : class
    {
        public BackendCommand(string method, Uri uri)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public string MediaType { get; set; } = "application/json";

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public TResult Result { get; private set; }

        public object Data { get; set; }

        public async override Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            if (Data == null) return;
            request.Content = new StringContent(await Task.Run(() => JsonConvert.SerializeObject(Data)).ConfigureAwait(false), Encoding, MediaType);
        }

        public async override Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            if (response.StatusCode == HttpStatusCode.NoContent) return;
            string content = await response.Content.ReadAsStringAsync();

            if (!(response.StatusCode == HttpStatusCode.OK
                || response.StatusCode == HttpStatusCode.Created))
                throw new HttpCommandException((int)response.StatusCode, content);

            await Task.Run(() =>
            {
               Result = JsonConvert.DeserializeObject<TResult>(content);
            }).ConfigureAwait(false);
        }
    }

}
