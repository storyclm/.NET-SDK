/*!
* StoryCLM.SDK Library v1.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoryCLM.SDK.Extensions;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public class SCLM
    {
        const string kMediaTypeHeader = "application/json";
        internal string kUsers = @"/v1/users/";
        internal string kTables = @"/v1/tables/";
        internal string kClients = @"/v1/clients/";
        internal string kPresentations = @"/v1/presentations/";
        internal string kMediafiles = @"/v1/mediafiles/";
        internal string kSlides = @"/v1/slides/";
        internal string kContentpackages = @"/v1/contentpackages/";
        const string kWebHooks = @"/v1/webhooks/";

        private string _clientId;
        private string _clientSecret;

        public StoryToken Token { get; set; }

        const string endpoint = "https://api.storyclm.com";
        const string authEndpoint = "https://auth.storyclm.com";

        #region CRUD

        private void ThrowResponseException(HttpResponseMessage response, string result)
        {
            if (response == null) return;
            throw new Exception($"Response: error; Status code: {response.StatusCode}; Message: {(string.IsNullOrEmpty(result) ? string.Empty : result)}");
        }

        internal async Task<string> POSTAsync(string resource, string content, string contentType = kMediaTypeHeader)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                HttpResponseMessage response = await client.PostAsync(resource, new StringContent(content, Encoding.UTF8, contentType));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
                result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != System.Net.HttpStatusCode.Created ||
                    response.StatusCode != System.Net.HttpStatusCode.OK)) ThrowResponseException(response, result);
            }
            return result;
        }

        internal async Task<T> POSTAsync<T>(string resource, object o)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                string c = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o));
                HttpResponseMessage response = await client.PostAsync(resource,
                    new StringContent(c, Encoding.UTF8, kMediaTypeHeader));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.Created) ThrowResponseException(response, result);
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        internal async Task<T> PUTAsync<T>(string resource, object o)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                HttpResponseMessage response = await client.PutAsync(resource,
                    new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        internal async Task PUTAsync(string resource, object o)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                HttpResponseMessage response = await client.PutAsync(resource,
                    new StringContent(o == null ? string.Empty : await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return;
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
        }

        internal async Task<T> GETAsync<T>(string resource, string query)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                HttpResponseMessage response = await client.GetAsync(resource + query);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        internal async Task<T> DELETEAsync<T>(string resource, string query)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                HttpResponseMessage response = await client.DeleteAsync(resource + query);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        internal async Task DELETEAsync(string resource, string query)
        {
            if (string.IsNullOrEmpty(resource)) throw new Exception("");
            string result = null;
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(endpoint) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
                await RefreshAsync();
                client.SetToken(Token);
                HttpResponseMessage response = await client.DeleteAsync(resource + query);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return;
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
        }

        #endregion

        #region Auth

        private async Task RefreshAsync()
        {
            if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret)) return;
            if (Token == null) return;
            if (string.IsNullOrEmpty(Token.RefreshToken) || Token.Expires == null) return;
            if (Token.Expires.Value <= DateTime.Now) await AuthAsync(_clientId, _clientSecret, Token.RefreshToken);
        }

        private async Task<StoryToken> AuthAsync(Dictionary<string, string> form)
        {
            StoryToken token = new StoryToken();
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.PostAsync(authEndpoint + "/connect/token", new FormUrlEncodedContent(form));
                var result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != System.Net.HttpStatusCode.Created || response.StatusCode != System.Net.HttpStatusCode.OK)) ThrowResponseException(response, result);
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
            Token = token;
            return token;
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

        #region Tables

        public async Task<IEnumerable<StoryTable<T>>> GetTablesAsync<T>(int clientId) where T : class, new()
        {
            IEnumerable<StoryTable<T>> result = await GETAsync<IEnumerable<StoryTable<T>>>(kTables + clientId + "/tables", string.Empty);
            foreach (var t in result) t._sclm = this;
            return result;
        }

        public async Task<StoryTable<T>> GetTableAsync<T>(int tableId) where T : class, new()
        {
            StoryTable<T> table = await GETAsync<StoryTable<T>>(kTables + tableId, string.Empty);
            table._sclm = this;
            return table;
        }

        #endregion

        #region Content

        public async Task<IEnumerable<StoryClient>> GetClientsAsync()
        {
            IEnumerable<StoryClient> result = await GETAsync<IEnumerable<StoryClient>>(kClients, string.Empty);
            foreach (var t in result) t._sclm = this;
            return result;
        }

        public async Task<StoryClient> GetClientAsync(int clientId)
        {
            StoryClient client = await GETAsync<StoryClient>(kClients + clientId, string.Empty);
            client._sclm = this;
            return client;
        }

        public async Task<StoryPresentation> GetPresentationAsync(int presentationId)
        {
            StoryPresentation presentation = await GETAsync<StoryPresentation>(kPresentations + presentationId, string.Empty);
            presentation._sclm = this;
            return presentation;
        }

        public async Task<StoryMediafile> GetMediafileAsync(int mediafileId) =>
            await GETAsync<StoryMediafile>(kMediafiles + mediafileId, string.Empty);

        public async Task<StorySlide> GetSlideAsync(int slideId) =>
            await GETAsync<StorySlide>(kSlides + slideId, string.Empty);

        public async Task<StoryPackageSas> GetContentPackageAsync(int presentationId) =>
            await GETAsync<StoryPackageSas>(kContentpackages + presentationId, string.Empty);

        #endregion

        #region Users

        public async Task<StoryUser> UserExistsAsync(string username)
        {
            StoryUser user = await GETAsync<StoryUser>(kUsers + "/exists?username=" + username, string.Empty);
            user._sclm = this;
            return user;
        }

        public async Task<StoryUser> CreateUserAsync(StoryCreateUserModel user)
        {
            StoryUser u = await POSTAsync<StoryUser>(kUsers, user);
            u._sclm = this;
            return u;
        }

        public async Task<StoryUser> GetUserAsync(string userId)
        {
            StoryUser user = await GETAsync<StoryUser>(kUsers + userId, string.Empty);
            user._sclm = this;
            return user;
        }

        public async Task<IEnumerable<StoryUserItem>> GetUsersAsync()
        {
            var users = await GETAsync<IEnumerable<StoryUserItem>>(kUsers, string.Empty);
            foreach (var t in users) t._sclm = this;
            return users;
        }

        #endregion
    }
}
