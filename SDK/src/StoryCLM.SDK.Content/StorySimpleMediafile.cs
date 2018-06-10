using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleMediafile : StorySimpleModel<StoryMediafile>
    {

        public async override Task<StoryMediafile> LoadAsync() =>
            await Context.GetMediafileAsync(Id);

    }
}
