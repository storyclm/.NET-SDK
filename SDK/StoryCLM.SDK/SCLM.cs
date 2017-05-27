/*!
* StoryCLM.SDK Library v1.0.0
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
        #region Singleton

        private static volatile SCLM instance;
        private static object syncRoot = new Object();

        private SCLM() { }

        public static SCLM Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SCLM();
                    }
                }
                return instance;
            }
        }

        #endregion 

        const string kMediaTypeHeader = "application/json";
        const string kTables = @"/v1/tables/";
        const string kWebHooks = @"/v1/webhooks/";

        public Token Token { get; private set; }

        const string endpoint = "https://api.storyclm.com";
        const string authEndpoint = "https://auth.storyclm.com";

        private string ToParamString(string[] param)
        {
            if (param == null) return string.Empty;
            if (param.Length == 0) return string.Empty;
            StringBuilder result = new StringBuilder("?");
            for (int i = 0; i < param.Length; i++)
            {
                result.Append("ids=");
                result.Append(param[i]);
                if (i != param.Length - 1)
                    result.Append("&");
            }
            return result.ToString();
        }

        private string ToBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        private long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }

        #region CRUD

        private void ThrowResponseException(HttpResponseMessage response, string result)
        {
            if (response == null) return;
            throw new Exception($"Response: error; Status code: {response.StatusCode}; Message: {(string.IsNullOrEmpty(result) ? string.Empty : result)}");
        }

        private async Task<string> POSTAsync(string resource, string content, string contentType = kMediaTypeHeader)
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
                if (!(response.StatusCode != System.Net.HttpStatusCode.Created ||
                    response.StatusCode != System.Net.HttpStatusCode.OK)) ThrowResponseException(response, result);
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        private async Task<T> POSTAsync<T>(string resource, object o)
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
                client.SetToken(Token);
                HttpResponseMessage response = await client.PostAsync(resource,
                    new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                if (response.StatusCode != System.Net.HttpStatusCode.Created) ThrowResponseException(response, result);
                result = await response.Content.ReadAsStringAsync();
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        private async Task<T> PUTAsync<T>(string resource, object o)
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
                client.SetToken(Token);
                HttpResponseMessage response = await client.PutAsync(resource,
                    new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
                result = await response.Content.ReadAsStringAsync();
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        private async Task<T> GETAsync<T>(string resource, string query)
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
                client.SetToken(Token);
                HttpResponseMessage response = await client.GetAsync(resource + query);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
                result = await response.Content.ReadAsStringAsync();
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        private async Task<T> DELETEAsync<T>(string resource, string query)
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
                client.SetToken(Token);
                HttpResponseMessage response = await client.DeleteAsync(resource + query);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(T);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
                result = await response.Content.ReadAsStringAsync();
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        #endregion

        #region Auth

        public async Task<Token> AuthAsync(string clientId, string secret)
        {
            Token token = null;
            var values = new Dictionary<string, string>()
                {
                    { "grant_type", "client_credentials"},
                    { "client_id", clientId},
                    { "client_secret", secret},
                };
            using (var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.PostAsync(authEndpoint + "/connect/token", new FormUrlEncodedContent(values));
                var result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != System.Net.HttpStatusCode.Created || response.StatusCode != System.Net.HttpStatusCode.OK)) ThrowResponseException(response, result);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(JObject.Parse(result)["error"].ToString());
                var t = JObject.Parse(result);
                token = new Token()
                {
                    AccessToken = t["access_token"].ToString(),
                    ExpiresIn = t["expires_in"].ToString(),
                    TokenType = t["token_type"].ToString(),
                };
            }
            Token = token;
            return token;
        }

        #endregion

        #region Tables

        /// <summary>
        /// Таблицы.
        /// Список таблиц для клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента в базе данных</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiTable>> GetTablesAsync(int clientId) =>
            await GETAsync<IEnumerable<ApiTable>>(kTables + clientId + "/tables", string.Empty);

        #region Count

        /// <summary>
        /// Таблицы.
        /// Колличество записей в таблице.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <returns></returns>
        public async Task<long> CountAsync(int tableId, string query = null) =>
            (await GETAsync<ApiCount>(kTables + tableId + "/count", string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Count;


        /// <summary>
        /// Таблицы.
        /// Колличество записей лога таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка. Необязательный параметр.</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(int tableId, DateTime? date = null) =>
            (await GETAsync<ApiCount>(kTables + tableId + "/logcount", date == null ? string.Empty : $"?date={ConvertToUnixTime(date.Value).ToString()}")).Count;

        #endregion

        #region Find

        /// <summary>
        /// Таблицы.
        /// Получить запись таблицы по идентификатору.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<T> FindAsync<T>(int tableId, string id) where T : class, new() =>
            await GETAsync<T>(kTables + tableId + "/findbyid/" + id, string.Empty);

        /// <summary>
        /// Таблицы.
        /// Найти записи в таблице по списку идентификаторов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync<T>(int tableId, IEnumerable<string> ids) where T : class, new() =>
            await GETAsync<IEnumerable<T>>(kTables + tableId + "/findbyids", ToParamString(ids.ToArray()));

        /// <summary>
        /// Таблицы.
        /// Получить записи по запросу.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="query">Запрос в формате TableQuery</param>
        /// <param name="sortfield">Поле, по которому нужно произвести сортировку</param>
        /// <param name="sort">Тип сортировки</param>
        /// <param name="skip">Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.</param>
        /// <param name="take">Максимальное количество записей, которые будут получены. По умолчанию - 100.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync<T>(int tableId, string query = null, string sortfield = null, int? sort = null, int skip = 0, int take = 100) where T : class, new()
        {
            string resource = kTables + tableId + $"/find?skip={skip}&take={take}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await GETAsync<IEnumerable<T>>(resource, string.Empty);
        }


        #endregion

        #region Log

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка. Необязательный параметр.</param>
        /// <param name="skip">Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.</param>
        /// <param name="take">Максимальное количество записей, которые будут получены. По умолчанию - 100.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiLog>> LogAsync(int tableId, DateTime? date = null, int skip = 0, int take = 100) =>
            await GETAsync<IEnumerable<ApiLog>>(kTables + tableId + $"/log?skip={skip}&take={take}" +
                (date == null ? string.Empty : $"&date={ConvertToUnixTime(date.Value).ToString()}"), string.Empty);

        #endregion

        #region Insert

        /// <summary>
        /// Таблицы.
        /// Добавляет новую запись в таблицу.
        /// </summary>
        /// <typeparam name="T">Тип новой записи</typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns>Новая запись с добавлением идендификатора</returns>
        public async Task<T> InsertAsync<T>(int tableId, T o) where T : class, new() =>
            await POSTAsync<T>(kTables + tableId + "/insert", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> InsertAsync<T>(int tableId, IEnumerable<T> o) where T : class, new() =>
            await POSTAsync<IEnumerable<T>>(kTables + tableId + "/insertmany", o);

        #endregion

        #region Update

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<T> UpdateAsync<T>(int tableId, T o) where T : class, new() =>
            await PUTAsync<T>(kTables + tableId + "/update", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UpdateAsync<T>(int tableId, IEnumerable<T> o) where T : class, new() =>
            await PUTAsync<IEnumerable<T>>(kTables + tableId + "/updatemany", o);

        #endregion

        #region Delete

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(int tableId, string id) where T : class, new() =>
            await DELETEAsync<T>(kTables + tableId + "/delete/" + id, string.Empty);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> DeleteAsync<T>(int tableId, IEnumerable<string> ids) where T : class, new() =>
            await DELETEAsync<IEnumerable<T>>(kTables + tableId + "/deletemany", ToParamString(ids.ToArray()));

        #endregion

        #region Aggregation

        private class AggregationResult<T>
        {
            public T Result { get; set; }
        }

        public async Task<T> MaxAsync<T>(int tableId, string field, string query = null) where T : IConvertible
        {
            return (await GETAsync<AggregationResult<T>>(kTables + tableId + "/max/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<T> MinAsync<T>(int tableId, string field, string query = null) where T : IConvertible
        {
            return (await GETAsync<AggregationResult<T>>(kTables + tableId + "/min/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<T> SumAsync<T>(int tableId, string field, string query = null) where T : IConvertible
        {
            return (await GETAsync<AggregationResult<T>>(kTables + tableId + "/sum/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<double> AvgAsync(int tableId, string field, string query = null)
        {
            return (await GETAsync<AggregationResult<double>>(kTables + tableId + "/avg/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        private async Task<T> FLAsync<T>(int tableId, bool first, string query = null, string sortfield = null, int? sort = null) where T : class, new()
        {
            var q = query == null && sortfield == null && sort == null ? "" : "?";
            var f = first ? "first" : "last";
            string resource = kTables + tableId + $"/{f}{q}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await GETAsync<T>(resource, string.Empty);
        }

        public async Task<T> FirstAsync<T>(int tableId, string query = null, string sortfield = null, int? sort = null) where T : class, new() =>
            await FLAsync<T>(tableId, true, query: query, sortfield: sortfield, sort: sort);

        public async Task<T> LastAsync<T>(int tableId, string query = null, string sortfield = null, int? sort = null) where T : class, new() =>
            await FLAsync<T>(tableId, false, query: query, sortfield: sortfield, sort: sort);


        #endregion

        #endregion


    }
}
