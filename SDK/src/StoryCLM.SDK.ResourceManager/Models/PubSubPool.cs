using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.ResourceManager.Models
{
    public class PubSubPool : Pool<PubSubResource>
    {
        internal PubSubPool() { }

        internal PubSubPool(SCLM sclm) =>
            _sclm = sclm;

        const string PUBSUB = "pubsub";

        public async override Task<PubSubResource> Allocate(string id)
        {
           var result = await _sclm.PUTAsync<PubSubResource>(GetUri($"{PUBSUB}/{id}"), null);
            result._sclm = _sclm;
            return result;
        }

        public async override Task<long> Count() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{PUBSUB}/count"))).Count;

        public async override Task<PubSubResource> Get(string id)
        {
            try
            {
                var result = await _sclm.GETAsync<PubSubResource>(GetUri($"{PUBSUB}/{id}"));
                result._sclm = _sclm;
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async override Task<long> Limit() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{PUBSUB}/limit"))).Count;

        public async override Task<IEnumerable<PubSubResource>> List()
        {
            var result = await _sclm.GETAsync<IEnumerable<PubSubResource>>(GetUri($"{PUBSUB}"));

            foreach (var t in result)
                t._sclm = _sclm;

            return result;
        }

        public async override Task Release(string id) =>
            await _sclm.DELETEAsync<object>(GetUri($"{PUBSUB}/{id}"));


    }
}
