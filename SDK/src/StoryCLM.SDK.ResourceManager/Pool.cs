using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.ResourceManager
{
    public abstract class Pool<T> : IResourcePool<T> where T : class, new()
    {
        const string VERSION = "v1";
        const string PATH = @"rm";
        const string API = "api";

        internal Uri GetUri(string query) =>
            new Uri($"{_sclm.GetEndpoint(API)}{VERSION}/{PATH}/{query}", UriKind.Absolute);

        internal SCLM _sclm;

        public abstract Task<T> Allocate(string id = null);

        public abstract Task<long> Count();

        public abstract Task<T> Get(string id);

        public abstract Task<long> Limit();

        public abstract Task<IEnumerable<T>> List();

        public abstract Task Release(string id);
    }
}
