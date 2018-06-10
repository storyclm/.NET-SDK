using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleMediafile : StorySimpleModel, ISCLMObject<StoryMediafile>
    {
        public SCLM Context { get; private set; }

        public async Task<StoryMediafile> LoadAsync() =>
            await Context.GetMediafileAsync(Id);

        public void SetContext(SCLM context) =>
            Context = context;
    }
}
