using Breffi.Story.Common.Retry;
using Shared;
using StoryCLM.SDK.Authentication;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Test
{
    public class RetryCommand : IHttpCommand
    {
        public RetryCommand(Exception ex)
        {
            Exception = ex;
        }

        public string Method { get; set; }
        public Uri Uri { get; set; }
        public TimeSpan ExpiryTime { get; set; } = TimeSpan.Zero;
        public Exception Exception { get; set; }

        public void Dispose() { }

        int Count;

        public async Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {
            if (++Count == 10)
            {
                Exception = null;
                return;
            }
            throw Exception;
        }

        public async Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
        }
    }

    public class RetryPolicy1
    {
        static Settings Settings => Settings.Get();

        [Fact]
        public async Task RetryCount()
        {
            SCLM sclm = new SCLM();
            await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret);
            var retryCommand = new RetryCommand(new TimeoutException())
            {
                Uri = new Uri($"{sclm.GetEndpoint("api")}v1/users/"),
                Method = "GET",
            };
            await sclm.ExecuteHttpCommand(retryCommand, new RetryPolicy()
            {
                ExponentialBackoff = true,
                Interval = TimeSpan.FromMilliseconds(50),
                RetriesCount = 10
            }, CancellationToken.None);
            Assert.NotNull(retryCommand.Exception);
        }

        [Fact]
        public async Task RetryLinar()
        {
            SCLM sclm = new SCLM();
            await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret);
            var retryCommand = new RetryCommand(new TimeoutException())
            {
                Uri = new Uri($"{sclm.GetEndpoint("api")}v1/users/"),
                Method = "GET",
            };
            await sclm.ExecuteHttpCommand(retryCommand, new RetryPolicy()
            {
                ExponentialBackoff = false,
                Interval = TimeSpan.FromMilliseconds(50),
                RetriesCount = 10
            }, CancellationToken.None);
            Assert.NotNull(retryCommand.Exception);
        }

    }
}
