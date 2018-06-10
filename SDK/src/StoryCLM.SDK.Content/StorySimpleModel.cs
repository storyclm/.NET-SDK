using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public abstract class StorySimpleModel<T> : StorySimpleModelBase, ISCLMObject<T>
    {
        public SCLM Context { get; private set; }

        public abstract Task<T> LoadAsync();

        public void SetContext(SCLM context) =>
            Context = context;
    }
}
