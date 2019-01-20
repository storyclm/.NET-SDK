using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Authentication
{
    public static class AuthenticationExtensions
    {
        const string auth = nameof(auth);

        internal static async Task<StoryToken> AuthAsync(Uri endpoint, Dictionary<string, string> form)
        {
            StoryToken token = new StoryToken();
            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.PostAsync(endpoint + "/connect/token", new FormUrlEncodedContent(form));
                var result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != HttpStatusCode.Created
                    || response.StatusCode != HttpStatusCode.OK)) throw new InvalidOperationException(result);
                if (response.StatusCode == HttpStatusCode.BadRequest) throw new Exception(JObject.Parse(result)["error"].ToString());
                JObject t = JObject.Parse(result);
                JToken accessToken = t["access_token"];
                if (accessToken != null) token.AccessToken = accessToken.Value<string>();
                JToken expiresIn = t["expires_in"];
                if (expiresIn != null) token.Expires = DateTime.Now + TimeSpan.FromSeconds(expiresIn.Value<int>());
                JToken tokenType = t["token_type"];
                if (tokenType != null) token.TokenType = tokenType.Value<string>();
                JToken refreshToken = t["refresh_token"];
                if (refreshToken != null) token.RefreshToken = refreshToken.Value<string>();
            }
            return token;
        }

        internal static async Task<StoryToken> AuthAsync(Uri endpoint, string clientId, string secret, string username = null, string password = null)
        {
            bool pc = !(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password));
            var values = new Dictionary<string, string>()
                {
                    { "grant_type", pc ? "password" : "client_credentials"},
                    { "client_id", clientId},
                    { "client_secret", secret},
                };
            if (pc)
            {
                values["username"] = username;
                values["password"] = password;
            }
            return await AuthAsync(endpoint, values);
        }


        internal static async Task<StoryToken> AuthAsync(Uri uri, string clientId, string secret, string refreshToken)
        {
            var values = new Dictionary<string, string>()
                {
                    { "grant_type", "refresh_token"},
                    { "client_id", clientId},
                    { "client_secret", secret},
                    { "refresh_token", refreshToken},
                };
            return await AuthAsync(uri, values);
        }

        internal static void SetDecorator(SCLM sclm, string clientId, string secret, StoryToken token) =>
            sclm.AddHttpDecorator<OAuthDecorator>(new OAuthDecorator
            {
                ClientId = clientId,
                Secret = secret,
                Token = token
            });

        public static async Task<StoryToken> AuthAsync(this SCLM sclm, string clientId, string secret, string username = null, string password = null)
        {
            StoryToken token = await AuthAsync(sclm.GetEndpoint(auth), clientId, secret, username, password);
            SetDecorator(sclm, clientId, secret, token);
            return token;
        }

        public static async Task<StoryToken> AuthAsync(this SCLM sclm, string clientId, string secret, string refreshToken)
        {
            StoryToken token = await AuthAsync(sclm.GetEndpoint(auth), clientId, secret, refreshToken);
            SetDecorator(sclm, clientId, secret, token);
            return token;
        }
    }
}
