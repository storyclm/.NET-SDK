using System;
using System.Collections.Generic;
using System.Text;

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
        public string this[string i]
        {
            get => Meta?[i];
            set
            {
                if (Meta == null) return;
                Meta[i] = value;
            }
        }

        public string Id { get; set; }

        public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();

        public DateTimeOffset? Date { get; set; }

        public string Hash { get; set; }

        public long? Lenght { get; set; }

        public Uri Uri { get; set; }

        public DateTimeOffset Expiration { get; set; }

        public MessageResultMode Mode { get; set; }
    }
}
