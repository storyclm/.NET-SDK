using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.ResourceManager.Models
{
    public class PubSubResource : Pool<SubscriptionResource>, IResource<IDictionary<string, string>>
    {
        const string PUBSUB = "pubsub";
        const string SUBSCRIPTION = "subscriptions";

        public string Id { get; set; }

        public string Group { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public async override Task<SubscriptionResource> Allocate(string id = null) =>
            await _sclm.PUTAsync<SubscriptionResource>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/{id}"), null);

        public async override Task<long> Count() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/count"))).Count;

        public async override Task<SubscriptionResource> Get(string id)
        {
            try
            {
                return await _sclm.GETAsync<SubscriptionResource>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/{id}"));
            }
            catch
            {
                return null;
            }
        }

        public async override Task<long> Limit() =>
            (await _sclm.GETAsync<StoryCount>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/limit"))).Count;

        public async override Task<IEnumerable<SubscriptionResource>> List() =>
            await _sclm.GETAsync<IEnumerable<SubscriptionResource>>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/"));

        public async override Task Release(string id) =>
            await _sclm.DELETEAsync<object>(GetUri($"{PUBSUB}/{Id}/{SUBSCRIPTION}/{id}"));

    }
}
