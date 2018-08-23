using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public abstract class PublishBase : ResultCommand<Message>
    {
        public PublishBase(string key, string secret, Stream data = null) : base(key, secret, data) { }

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

        public IoTMessageType MessageType { get; protected set; } = IoTMessageType.Command;

    }
}
