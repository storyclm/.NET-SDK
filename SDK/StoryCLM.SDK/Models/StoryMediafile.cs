using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StoryMediafile : StorySimpleModel
    {
        internal StoryMediafile() { }

        public string FileName { get; set; }

        public string Title { get; set; }

        public string BlobId { get; set; }

        public int FileSize { get; set; }

        public string MIMEType { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string SAS { get; set; }

        public bool IsAvailableForSharing { get; set; }
    }
}
