using Shared;
using StoryCLM.SDK.ResourceManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.ResourceManager.Test
{
    public class RM
    {
        const string QUEUE = "testqueue";
        const string PUBSUB = "testpubsub";
        const string SUBSCRIPTION = "testsubscription";

        [Fact]
        public async Task Queue()
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            QueuesPool pool = sclm.GetQueuesPool();
            QueueResource queue = await pool.Allocate(QUEUE);
            Assert.NotNull(queue);
            Assert.Equal(QUEUE, queue.Id);

            IEnumerable<QueueResource> list = await pool.List();
            Assert.NotEmpty(list);
            Assert.Contains(queue.Id, list.Select(t => t.Id));

            Assert.True((await pool.Count()) > 0);

            QueueResource queueGet = await pool.Get(QUEUE);
            Assert.NotNull(queueGet);
            Assert.Equal(QUEUE, queueGet.Id);

            await pool.Release(QUEUE);
            QueueResource queueDeleted = await pool.Get(QUEUE);
            Assert.Null(queueDeleted);
        }

        [Fact]
        public async Task PubSub()
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            PubSubPool pool = sclm.GetPubSubPool();
            PubSubResource pubsub = await pool.Allocate(PUBSUB);
            Assert.NotNull(pubsub);
            Assert.Equal(PUBSUB, pubsub.Id);

            IEnumerable<PubSubResource> list = await pool.List();
            Assert.NotEmpty(list);
            Assert.Contains(pubsub.Id, list.Select(t => t.Id));

            Assert.True((await pool.Count()) > 0);

            PubSubResource pubSubGet = await pool.Get(PUBSUB);
            Assert.NotNull(pubSubGet);
            Assert.Equal(PUBSUB, pubSubGet.Id);

            //====================================SUBSCRIPTION

            SubscriptionResource sub = await pubSubGet.Allocate(SUBSCRIPTION);
            Assert.NotNull(sub);
            Assert.Equal(SUBSCRIPTION, sub.Id);

            IEnumerable<SubscriptionResource> slist = await pubSubGet.List();
            Assert.NotEmpty(slist);
            Assert.Contains(sub.Id, slist.Select(t => t.Id));

            Assert.True((await pubSubGet.Count()) > 0);

            SubscriptionResource subGet = await pubSubGet.Get(SUBSCRIPTION);
            Assert.NotNull(subGet);
            Assert.Equal(SUBSCRIPTION, subGet.Id);

            await pubSubGet.Release(SUBSCRIPTION);
            SubscriptionResource subDeleted = await pubSubGet.Get(SUBSCRIPTION);
            Assert.Null(subDeleted);

            //================================================

            await pool.Release(PUBSUB);
            PubSubResource pubSubDeleted = await pool.Get(PUBSUB);
            Assert.Null(pubSubDeleted);
        }
    }
}
