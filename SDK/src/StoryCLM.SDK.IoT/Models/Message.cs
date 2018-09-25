using SroryCLM.SDK.Common.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT.Models
{
    public enum MessageResultMode
    {
        Read,
        Write
    }

    #warning дублируется код
    public class Message
    {
        const string IOT = "iot";

        static readonly HttpClient _httpClient;

        static Message()
        {
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            });
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
        }

        internal Message() {}

        public string this[string i]
        {
            get => Meta?[i];
            set
            {
                if (Meta == null) return;
                Meta[i] = value;
            }
        }

        #region internal

        internal SCLM _sclm { get; set; }

        internal IoTParameters _parameters { get; set; }

        #endregion

        #region Base

        public string Id { get; set; }

        public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();

        public DateTimeOffset? Date { get; set; }

        public string Hash { get; set; }

        public long? Lenght { get; set; }

        public Uri Uri { get; set; }

        public DateTimeOffset Expiration { get; set; }

        public MessageResultMode Mode { get; set; }

        #endregion

        Uri _endpoint => new Uri($"{_sclm.GetEndpoint(IOT)}storage/{Id}/");

        public async Task Delete()
        {
            if (string.IsNullOrEmpty(Id)) throw new ArgumentNullException(nameof(Id));
            if (_sclm == null || _parameters == null) throw new InvalidOperationException();
            var command = new MessageCommand<Message>(_parameters)
            {
                Method = "DELETE",
                Endpoint = _endpoint
            };
            await _sclm.ExecuteHttpCommand(command, _sclm.HttpQueryPolicy);
        }

        async Task Update()
        {
            if (string.IsNullOrEmpty(Id)) throw new ArgumentNullException(nameof(Id));
            if (_sclm == null || _parameters == null) throw new InvalidOperationException();
            var command = new MessageCommand<Message>(_parameters)
            {
                Endpoint = _endpoint
            };
            await _sclm.ExecuteHttpCommand(command, _sclm.HttpQueryPolicy);
            Mode = command.Result.Mode;
            Hash = command.Result.Hash;
            Uri = command.Result.Uri;
            Expiration = command.Result.Expiration;
            Lenght = command.Result.Lenght;
            Meta = command.Result.Meta;
        }

        public async Task Save(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (Mode == MessageResultMode.Write 
                || !Lenght.HasValue
                || !stream.CanWrite
                || Uri == null
                || string.IsNullOrEmpty(Hash))
                throw new InvalidOperationException("Invalid message.");

            Retry retry = new Retry();
            await retry.Execute(async () =>
            {
                if (Expiration <= DateTimeOffset.UtcNow)
                    await Update();

                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);

                using (SHA256 sha256 = SHA256.Create())
                using (Stream response = await _httpClient.GetStreamAsync(Uri).ConfigureAwait(false))
                {
                    byte[] buffer = new byte[8 * 1024];
                    int lenght = 0;
                    long threshold = Lenght.Value;
                    while ((lenght = await response.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        await stream.WriteAsync(buffer, 0, lenght);
                        if (stream.Length > threshold) throw new InvalidOperationException("Message body too large.");
                        sha256.TransformBlock(buffer, 0, lenght, null, 0);
                    }
                    sha256.TransformFinalBlock(buffer, 0, 0);
                    if(Hash != Convert.ToBase64String(sha256.Hash)) throw new InvalidOperationException("Wrong hash.");
                }

            }, _sclm.HttpQueryPolicy);
        }
    }
}
