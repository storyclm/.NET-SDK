using System;

namespace StoryCLM.SDK.Content
{
    public class StorySlide : StorySimpleModelBase
    {
        internal StorySlide() { }

        public string Name { get; set; }

        public string PageSource { get; set; }

        public string LinkedSlides { get; set; }

        public string SwipeNext { get; set; }

        public string SwipePrevious { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

    }
}
