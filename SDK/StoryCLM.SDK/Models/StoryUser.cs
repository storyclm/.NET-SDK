using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StoryUser
    {
        public StoryUser()
        {
            Clients = Enumerable.Empty<StoryClientRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public IEnumerable<StoryClientRole> Clients { get; set; }
    }
}
