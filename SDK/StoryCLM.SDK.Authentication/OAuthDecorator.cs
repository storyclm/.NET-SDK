using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Authentication
{
    public class OAuthDecorator : IHttpPiplineDecorator
    {
        public StoryToken Token { get; set; }

        public string ClientId { get; set;}

        public string Secret { get; set; }

        public async Task OnExecuting(HttpPiplineRequest context)
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

        public async Task OnExecuted(HttpPiplineResponse context) {}
    }
}
