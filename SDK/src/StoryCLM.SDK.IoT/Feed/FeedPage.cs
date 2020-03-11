using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public class FeedPage
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
