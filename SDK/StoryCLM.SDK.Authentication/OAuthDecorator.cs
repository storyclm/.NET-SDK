using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Authentication
{
    public class OAuthDecorator : IHttpPiplineDecorator
    {
        public StoryToken Token { get; set; }

        public string ClientId { get; set;}

        public string Secret { get; set; }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public async Task OnExecuting(HttpPiplineRequest context)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                if (string.IsNullOrEmpty(ClientId)
                    || string.IsNullOrEmpty(Secret)
                    || Token == null
                    || Token.Expires == null) return;

                if (Token.Expires.Value <= DateTime.Now)
                    if (string.IsNullOrEmpty(Token.RefreshToken))
                        Token = await AuthenticationExtensions.AuthAsync(context.Executor.GetEndpoint("auth"), ClientId, Secret);
                    else
                        Token = await AuthenticationExtensions.AuthAsync(context.Executor.GetEndpoint("auth"), ClientId, Secret, Token.RefreshToken);

                context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token?.AccessToken ?? string.Empty);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task OnExecuted(HttpPiplineResponse context) {}
    }
}
