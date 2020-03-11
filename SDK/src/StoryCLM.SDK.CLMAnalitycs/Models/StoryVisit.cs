using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.CLMAnalitycs.Models
{
   public class StoryVisit<T> : StorySessionEvent
    {
        public IEnumerable<StorySlideEvent> Slides { get; set; }

        public T CustomEvents { get; set; }
    }
}
