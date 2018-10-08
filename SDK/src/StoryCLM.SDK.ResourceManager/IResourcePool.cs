using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.ResourceManager
{
    public interface IResourcePool<TResource>  where TResource : class, new()
    {
        Task<TResource> Allocate(string id);

        Task<TResource> Get(string id);

        Task Release(string id);

        Task<IEnumerable<TResource>> List();

        Task<long> Count();

        Task<long> Limit();

    }
}
