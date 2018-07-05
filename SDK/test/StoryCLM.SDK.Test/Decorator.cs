using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StoryCLM.SDK.Authentication;
using Shared;
using System.Threading;

namespace StoryCLM.SDK.Test
{
    public class TestDecorator : IHttpPiplineDecorator
    {
        public bool Executing { get; set; }
        public bool Executed { get; set; }

        public async Task OnExecuting(HttpPiplineRequest context)
        {
            Assert.NotNull(context);
            Assert.NotNull(context.Command);
            Assert.NotNull(context.Executor);
            Assert.NotNull(context.Parameters);
            Assert.NotNull(context.Request);
            Executing = true;
        }

        public async Task OnExecuted(HttpPiplineResponse context)
        {
            Assert.NotNull(context);
            Assert.NotNull(context.Command);
            Assert.NotNull(context.Executor);
            Assert.NotNull(context.Parameters);
            Assert.NotNull(context.Response);
            Executed = true;
        }
    }

    public class Decorator
    {
        [Fact]
        public async Task Pipeline()
        {
            TestDecorator testDecorator = new TestDecorator();
            SCLM sclm = new SCLM();
            sclm.AddHttpDecorator(testDecorator);
            Assert.NotNull(sclm.GetHttpDecorator<TestDecorator>());

            await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret);
            IEnumerable<object> objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);
            Assert.True(testDecorator.Executed && testDecorator.Executing);
        }
    }
}
