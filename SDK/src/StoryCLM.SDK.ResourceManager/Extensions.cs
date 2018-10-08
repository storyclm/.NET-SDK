using StoryCLM.SDK.ResourceManager.Models;
using System;

namespace StoryCLM.SDK.ResourceManager
{
    public static class Extensions
    {
        public static QueuesPool GetQueuesPool(this SCLM sclm) => new QueuesPool(sclm);

        public static PubSubPool GetPubSubPool(this SCLM sclm) => new PubSubPool(sclm);
    }
}
