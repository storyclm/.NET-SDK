using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT.Models
{
    public class FeedPage
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
