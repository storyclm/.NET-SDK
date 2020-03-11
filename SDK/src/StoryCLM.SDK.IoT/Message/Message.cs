using Breffi.Story.Common.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    #warning дублируется код
    public class Message
    {
        const string IOT = "iot";
        const string DELETEEX = "Message has been deleted";

        static readonly HttpClient _httpClient;

        Uri _endpoint => new Uri($"{_parameters.Endpoint}{_parameters.Hub}/storage/{Id}/");

        DateTimeOffset _expiration;

        bool _deleted;

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

        internal Message()
        {
            _expiration = DateTimeOffset.UtcNow.AddDays(1).AddMinutes(-5);
        }

        public string this[string i]
        {
            get => Metadata?[i];
            set
            {
                if (Metadata == null) return;
                Metadata[i] = value;
            }
        }

        internal SCLM _sclm { get; set; }

        internal IoTParameters _parameters { get; set; }

        public virtual string Id { get; set; }

        public virtual IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public virtual long? Ticks { get; set; }

        public virtual Uri Location { get; set; }

        public virtual string Signature { get; set; }

        public virtual string Hash { get; set; }

        public virtual long? Lenght { get; set; }

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
            if (command.Exception == null)
                _deleted = true;
        }

        public async Task Confirm()
        {
            if (string.IsNullOrEmpty(Id)) throw new ArgumentNullException(nameof(Id));
            if (_sclm == null || _parameters == null) throw new InvalidOperationException();
            var command = new MessageCommand<Message>(_parameters)
            {
                Method = "PUT",
                Endpoint = new Uri($"{_parameters.Endpoint}{_parameters.Hub}/publish/{Id}/confirm")
            };
            command.SetParameter("hash", Hash);
            await _sclm.ExecuteHttpCommand(command, _sclm.HttpCommandPolicy);
            Up(command);
        }

        void Up(MessageCommand<Message> command)
        {
            if (command.Exception != null) throw command.Exception;
            Location = command.Result.Location;
            Hash = command.Result.Hash;
            Ticks = command.Result.Ticks;
            Signature = command.Result.Signature;
            Lenght = command.Result.Lenght;
            Metadata = command.Result.Metadata;
        }

        async Task Update()
        {
            if(_deleted) throw new InvalidOperationException(DELETEEX);
            if (string.IsNullOrEmpty(Id)) throw new ArgumentNullException(nameof(Id));
            if (_sclm == null || _parameters == null) throw new InvalidOperationException();
            var command = new MessageCommand<Message>(_parameters)
            {
                Endpoint = _endpoint
            };
            await _sclm.ExecuteHttpCommand(command, _sclm.HttpQueryPolicy);
            _expiration = DateTimeOffset.UtcNow.AddDays(1).AddMinutes(-5);
            Up(command);
        }

        public async Task Save(Stream stream)
        {
            if (_deleted) throw new InvalidOperationException(DELETEEX);
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            Retry retry = new Retry();
            await retry.Execute(async () =>
            {
                if (_expiration <= DateTimeOffset.UtcNow)
                    await Update();

                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);

                using (SHA512 sha512 = SHA512.Create())
                using (Stream response = await _httpClient.GetStreamAsync(Location).ConfigureAwait(false))
                {
                    byte[] buffer = new byte[8 * 1024];
                    int lenght = 0;
                    long threshold = Lenght.Value;
                    while ((lenght = await response.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        await stream.WriteAsync(buffer, 0, lenght);
                        if (stream.Length > threshold) throw new InvalidOperationException("Message body too large");
                        sha512.TransformBlock(buffer, 0, lenght, null, 0);
                    }
                    sha512.TransformFinalBlock(buffer, 0, 0);
                    if(Hash != sha512.Hash.ToHashString()) throw new InvalidOperationException("Wrong hash");
                }

            }, _sclm.HttpQueryPolicy);
        }
    }
}
