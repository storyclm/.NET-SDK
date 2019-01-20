using Breffi.Story.Common.Retry;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public static class Extensions
    {
        const string IOT = "iot";
        const string PATH = "publish";

        static async Task<byte[]> CreateMessage(Stream stream, long threshold = 256 * 1024, CancellationToken token = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        if (tokenSource.IsCancellationRequested) throw new OperationCanceledException(token);
                        await ms.WriteAsync(buffer, 0, read);
                        if (ms.Length > threshold) throw new InvalidOperationException($"Threshold: {threshold}.");
                    }
                    return ms.ToArray();
                }
            }
        }

        public static async Task<Message> Send(SCLM sclm,
            IoTParameters parameters,
            byte[] body,
            IDictionary<string, string> meta = null,
            CancellationToken token = default(CancellationToken))
        {
            Retry retry = new Retry();
            return await retry.Execute(async () =>
            {
                using (MemoryStream stream = new MemoryStream(body ?? new byte[] { }))
                {
                    var command = new PublishMessage(parameters, stream);
                    command.Endpoint = new Uri($"{sclm.GetEndpoint(IOT)}{parameters.Hub}/{PATH}");

                    if (meta != null)
                        foreach (var t in meta)
                            command[t.Key] = t.Value;

                    await sclm.ExecuteHttpCommand(command, cancellationToken: token).ConfigureAwait(false);
                    command.Result._parameters = parameters;
                    command.Result._sclm = sclm;
                    return command.Result;
                }
            }, sclm.HttpQueryPolicy, token);
        }

        static async Task<string> UploadChank(Uri url, 
            byte[] body, 
            IRetryPolicy policy,
            CancellationToken token = default(CancellationToken))
        {
            Retry retry = new Retry();
            return await retry.Execute(async () =>
            {
                using (SHA512 sha512 = SHA512.Create())
                using (MemoryStream b = new MemoryStream(body))
                using (HashableStream stream = new HashableStream(b, sha512))
                {
                    StreamContent content = new StreamContent(stream, 1024 * 64);
                    content.Headers.ContentLength = stream.Length;
                    content.Headers.Add("x-ms-blob-type", "BlockBlob");
                    using (var httpClient = new HttpClient())
                    using (var httpRequest = new HttpRequestMessage(HttpMethod.Put, url) { Content = content })
                    using (HttpResponseMessage response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.StatusCode != HttpStatusCode.Created) new HttpCommandException((int)response.StatusCode, null);
                        return $"base64;sha512;{Convert.ToBase64String(stream.Hash)}";
                    }
                }
            }, policy, token);
        }

        public static async Task<Message> Publish(this SCLM sclm,
            IoTParameters parameters,
            byte[] body,
            IDictionary<string, string> meta = null,
            CancellationToken token = default(CancellationToken))
        {
            if (body == null || body.Length <= 256 * 1024)
                return await Send(sclm, parameters, body, meta, token);

            var message = await Send(sclm, parameters, null, meta, token);
            message.Hash = await UploadChank(message.Location, body, sclm.HttpQueryPolicy, token);
            await message.Confirm();
            return message;
        }

        public static Feed GetFeed(this SCLM sclm,
            IoTParameters parameters,
            string continuationToken = null, 
            Section section = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new Feed(continuationToken)
            {
                _sclm = sclm,
                CancellationToken = cancellationToken,
                _parameters = parameters,
                _section = section
            };
        }
    }
}
