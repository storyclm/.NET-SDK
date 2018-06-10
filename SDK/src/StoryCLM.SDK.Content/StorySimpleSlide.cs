using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleSlide : StorySimpleModel, ISCLMObject<StorySlide>
    {
        public SCLM Context { get; private set; }

        public async Task<StorySlide> LoadAsync() =>
            await Context.GetSlideAsync(Id);

        public void SetContext(SCLM context) =>
            Context = context;
    }
}
