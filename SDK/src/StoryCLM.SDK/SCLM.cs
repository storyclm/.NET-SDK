/*!
* StoryCLM.SDK Library v1.6.6
* Copyright(c) 2018, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoryCLM.SDK.Extensions;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public class SCLM
    {
        const string _api = "api";
        const string _auth = "auth";

        const string kMediaTypeHeader = "application/json";

        const string _post = "POST";
        const string _put = "PUT";
        const string _get = "GET";
        const string _delete = "DELETE";

        readonly IDictionary<string, Uri> _endpoints = new Dictionary<string, Uri>();
        static readonly HttpClient _httpClient;

        string _clientId;
        string _clientSecret;

        public StoryToken Token { get; set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        static SCLM()
        {
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            });
        }

        public SCLM()
        {
            SetEndpoint(_api, "https://api.storyclm.com");
            SetEndpoint(_auth, "https://auth.storyclm.com");
        }

        #region Endpoints

        public void SetEndpoint(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri)) throw new ArgumentException(nameof(value));
            _endpoints[name] = uri;
        }

        public Uri GetEndpoint(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            return _endpoints[name];
        }

        public Uri Endpoint => _endpoints[_api];

        public Uri AuthEndpoint
        {
            get => _endpoints[_auth];
        }

        #endregion

        #region CRUD

        public virtual async Task<T> SendAsync<T>(string method,
            Uri uri,
            object o = null,
            Action<HttpRequestMessage> OnExecuting = null,
            Func<HttpResponseMessage, T> OnExecuted = null,
            string contentType = kMediaTypeHeader)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            _httpClient.DefaultRequestHeaders.Clear();
            await RefreshTokenAsync();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token?.AccessToken ?? string.Empty);
            if (OnExecuting == null)
            {
                if (o != null)
                    request.Content = new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)).ConfigureAwait(false), Encoding, contentType);
            }
            else
                OnExecuting(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (OnExecuted == null)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) return default(T);
                string content = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)) throw new InvalidOperationException($"Code: {response.StatusCode}; {content}");

                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(content)).ConfigureAwait(false);
            }
            else
                return OnExecuted(response);
        }

        public async Task<T> POSTAsync<T>(Uri uri, object o)
            => await SendAsync<T>(_post, uri, o: o);

        public async Task<T> PUTAsync<T>(Uri uri, object o) 
            => await SendAsync<T>(_put, uri, o: o);

        public async Task PUTAsync(Uri uri, object o) 
            => await SendAsync<string>(_put, uri, o: o);

        public async Task<T> GETAsync<T>(Uri uri) 
            => await SendAsync<T>(_get, uri);

        public async Task<T> DELETEAsync<T>(Uri uri)
            => await SendAsync<T>(_delete, uri);

        public async Task DELETEAsync(Uri uri) 
            => await SendAsync<string>(_delete, uri);

        #endregion

        #region Auth

        public async Task RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_clientId) 
                || string.IsNullOrEmpty(_clientSecret)
                || Token == null
                || Token.Expires == null) return;
            if (Token.Expires.Value <= DateTime.Now)
                if(string.IsNullOrEmpty(Token.RefreshToken))
                    await AuthAsync(_clientId, _clientSecret);
                else
                    await AuthAsync(_clientId, _clientSecret, Token.RefreshToken);
        }

        private async Task<StoryToken> AuthAsync(Dictionary<string, string> form)
        {
            StoryToken token = new StoryToken();
            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.PostAsync(AuthEndpoint + "/connect/token", new FormUrlEncodedContent(form));
                var result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != HttpStatusCode.Created
                    || response.StatusCode != HttpStatusCode.OK)) throw new InvalidOperationException(result);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(JObject.Parse(result)["error"].ToString());
                JObject t = JObject.Parse(result);
                JToken accessToken = t["access_token"];
                if(accessToken != null) token.AccessToken = accessToken.Value<string>();
                JToken expiresIn = t["expires_in"];
                if (expiresIn != null) token.Expires = DateTime.Now + TimeSpan.FromSeconds(expiresIn.Value<int>());
                JToken tokenType = t["token_type"];
                if (tokenType != null) token.TokenType = tokenType.Value<string>();
                JToken refreshToken = t["refresh_token"];
                if (refreshToken != null) token.RefreshToken = refreshToken.Value<string>();
            }
            return Token = token;
        }

        public async Task<StoryToken> AuthAsync(string clientId, string secret, string username = null, string password = null)
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
            _clientId = clientId;
            _clientSecret = secret;
            return await AuthAsync(values);
        }

        public async Task<StoryToken> AuthAsync(string clientId, string secret, string refreshToken)
        {
            var values = new Dictionary<string, string>()
                {
                    { "grant_type", "refresh_token"},
                    { "client_id", clientId},
                    { "client_secret", secret},
                    { "refresh_token", refreshToken},
                };
            _clientId = clientId;
            _clientSecret = secret;
            return await AuthAsync(values);
        }

        #endregion

    }
}
