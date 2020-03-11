using Shared;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Test
{
    public class TestCommand : IHttpCommand
    {

        public string Method { get; set; }
        public Uri Uri { get; set; }
        public TimeSpan ExpiryTime { get; set; } = TimeSpan.Zero;
        public Exception Exception { get; set; }

        public void Dispose() {}

        public async Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {
            Assert.NotNull(request);
        }

        public async Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            Assert.NotNull(response);
            Result = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(Result);
        }

        public string Result { get; set; }
    }

    public class Command
    {
        static Settings Settings => Settings.Get();

        [Fact]
        public async Task ExecuteCommand()
        {
            //SCLM sclm = new SCLM();
            //await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret);
            //var testCommand = new TestCommand
            //{
            //    Uri = new Uri($"{sclm.GetEndpoint("api")}/v1/users/"),
            //    Method = "GET",
            //};
            //await sclm.ExecuteHttpCommand(testCommand, null, CancellationToken.None);
            //Assert.Null(testCommand.Exception);
        }
    }
}
