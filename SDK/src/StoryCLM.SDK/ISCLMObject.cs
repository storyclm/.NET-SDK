using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public interface ISCLMObject<T>
    {
        SCLM Context { get; }

        void SetContext(SCLM context);

        Task<T> LoadAsync();
    }
}
