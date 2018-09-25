using SroryCLM.SDK.Common.Retry;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public static class Extensions
    {
        const string IOT = "iot";
        const string PATH = "publish";

        static async Task<byte[]> CreateMessage(Stream stream, 
            long threshold = 128 * 1024, 
            CancellationToken token = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                byte[] buffer = new byte[4 * 1024];
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

        static async Task<Message> PublishMessage(SCLM sclm,
            IoTParameters parameters,
            byte[] body,
            IDictionary<string, string> meta = null,
            CancellationToken token = default(CancellationToken))
        {
            Retry retry = new Retry();
            return await retry.Execute(async () =>
            {
                using (MemoryStream stream = new MemoryStream(body)) // сообщение должно быть неизменяемое, что бы можно было повторить попытку отправки. Потому кешим его в байт массив и потом опять в стрим при каждой попытке.
                {
                    var command = new PublishMessage(parameters, stream);
                    command.Endpoint = new Uri(sclm.GetEndpoint(IOT) + PATH);

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

        public static async Task<Message> PublishCommand(this SCLM sclm,
            IoTParameters parameters,
            Stream body,
            IDictionary<string, string> meta = null,
            CancellationToken token = default(CancellationToken)) =>
            await PublishMessage(sclm, parameters, await CreateMessage(body, token: token), meta, token).ConfigureAwait(false);

        public static async Task<Message> PublishEvent(this SCLM sclm,
            IoTParameters parameters,
            Stream body,
            IDictionary<string, string> meta = null,
            CancellationToken token = default(CancellationToken)) =>
            await PublishMessage(sclm, parameters, await CreateMessage(body, 256 * 1024, token), meta, token).ConfigureAwait(false);

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
