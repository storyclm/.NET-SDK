using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.ResourceManager.Models
{
    public class QueuesPool : Pool<QueueResource>
    {
        internal QueuesPool() { }

        internal QueuesPool(SCLM sclm) =>
            _sclm = sclm;

        const string QUEUES = "queues";

        public async override Task<QueueResource> Allocate(string id) =>
            await _sclm.PUTAsync<QueueResource>(GetUri($"{QUEUES}/{id}"), null);

        public async override Task<long> Count() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{QUEUES}/count"))).Count;

        public async override Task<QueueResource> Get(string id)
        {
            try
            {
                return await _sclm.GETAsync<QueueResource>(GetUri($"{QUEUES}/{id}"));
            }
            catch
            {
                return null;
            }
        }

        public async override Task<long> Limit() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{QUEUES}/limit"))).Count;

        public async override Task<IEnumerable<QueueResource>> List() =>
            await _sclm.GETAsync<IEnumerable<QueueResource>>(GetUri($"{QUEUES}"));

        public async override Task Release(string id) =>
            await _sclm.DELETEAsync<object>(GetUri($"{QUEUES}/{id}"));
    }
}
