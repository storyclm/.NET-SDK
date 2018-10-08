using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.ResourceManager
{
    public interface IResource<TProperties>
    {
        string Id { get; set; }

        string Group { get; set; }

        TProperties Properties { get; set; }
    }
}
