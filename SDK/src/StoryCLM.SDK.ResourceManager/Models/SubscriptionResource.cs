using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.ResourceManager.Models
{
    public class SubscriptionResource : IResource<IDictionary<string, string>>
    {
        public string Id { get; set; }

        public string Group { get; set; }

        public IDictionary<string, string> Properties { get; set; }

    }
}
