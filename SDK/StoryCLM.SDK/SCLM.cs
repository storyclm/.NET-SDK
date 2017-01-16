/*!
* StoryCLM.SDK Library v0.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
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

        private SCLM() {}

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
        //const string endpoint = "https://api.storyclm.com";
        const string endpoint = "https://localhost:44330";

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
                result = await response.Content.ReadAsStringAsync();
                if (!(response.StatusCode != System.Net.HttpStatusCode.Created ||
                    response.StatusCode != System.Net.HttpStatusCode.OK)) ThrowResponseException(response, result);
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
                HttpResponseMessage response = await client.PostAsync(resource, 
                    new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.Created) ThrowResponseException(response, result);
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
                HttpResponseMessage response = await client.PutAsync(resource,
                    new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o)), Encoding.UTF8, kMediaTypeHeader));
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
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
                HttpResponseMessage response = await client.GetAsync(resource + query);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
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
                HttpResponseMessage response = await client.DeleteAsync(resource + query);
                result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK) ThrowResponseException(response, result);
            }
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result));
        }

        #endregion

        #region Tables

        /// <summary>
        /// Таблицы.
        /// Список таблиц для клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента в базе данных</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiTable>> GetTablesAsync(int clientId) => await GETAsync<IEnumerable<ApiTable>>(kTables + clientId + "/tables", string.Empty);

        #region Count

        /// <summary>
        /// Таблицы.
        /// Колличество записей в таблице.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <returns></returns>
        public async Task<long> CountAsync(int tableId) => (await GETAsync<ApiCount>(kTables + tableId + "/count", string.Empty)).Count;

        /// <summary>
        /// Таблицы.
        /// Колличество записей в таблице по запросу.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <returns></returns>
        public async Task<long> CountAsync(int tableId, string query) => (await GETAsync<ApiCount>(kTables + tableId + "/countbyquery/" + ToBase64(query), string.Empty)).Count;

        /// <summary>
        /// Таблицы.
        /// Колличество записей лога таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(int tableId) => (await GETAsync<ApiCount>(kTables + tableId + "/logcount", string.Empty)).Count;

        /// <summary>
        /// Таблицы.
        /// Колличество записей лога после указанной даты.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(int tableId, DateTime date) => (await GETAsync<ApiCount>(kTables + tableId + "/logcountbydate/" + ConvertToUnixTime(date).ToString(), string.Empty)).Count;

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
        public async Task<T> FindAsync<T>(int tableId, string id) => await GETAsync<T>(kTables + tableId + "/findbyid/" + id, string.Empty);


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
        public async Task<IEnumerable<T>> FindAsync<T>(int tableId, string query, string sortfield, int sort = 1, int skip = 0, int take = 100)
        {
            string resource = kTables + tableId + "/find/" + query + "/" + sortfield + "/" + sort.ToString() + "/" + skip + "/" + take;
            return await GETAsync<IEnumerable<T>>(resource, string.Empty);
        }

        /// <summary>
        /// Таблицы.
        /// Найти записи в таблице по списку идентификаторов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync<T>(int tableId, IEnumerable<string> ids) => await GETAsync<IEnumerable<T>>(kTables + tableId + "/findbyids", ToParamString(ids.ToArray()));

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="skip">Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.</param>
        /// <param name="take">Максимальное количество записей, которые будут получены. По умолчанию - 100.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync<T>(int tableId, int skip = 0, int take = 100) => await GETAsync<IEnumerable<T>>(kTables + tableId + "/findall/" + skip + "/" + take, string.Empty);

        #endregion

        #region Log

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="skip">Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.</param>
        /// <param name="take">Максимальное количество записей, которые будут получены. По умолчанию - 100.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiLog>> LogAsync(int tableId, int skip = 0, int take = 100) => await GETAsync<IEnumerable<ApiLog>> (kTables + tableId + "/log/" + skip + "/" + take, string.Empty);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка</param>
        /// <param name="skip">Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.</param>
        /// <param name="take">Максимальное количество записей, которые будут получены. По умолчанию - 100.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiLog>> LogAsync(int tableId, DateTime date, int skip = 0, int take = 100) => await GETAsync<IEnumerable<ApiLog>>(kTables + tableId + "/logbydate/" + ConvertToUnixTime(date).ToString() + "/" + skip + "/" + take, string.Empty);

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
        public async Task<T> InsertAsync<T>(int tableId, T o) => await POSTAsync<T>(kTables + tableId + "/insert", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> InsertAsync<T>(int tableId, IEnumerable<T> o) => await POSTAsync<IEnumerable<T>>(kTables + tableId + "/insertmany", o);

        #endregion

        #region Update

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<T> UpdateAsync<T>(int tableId, T o) => await PUTAsync<T>(kTables + tableId + "/update", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UpdateAsync<T>(int tableId, IEnumerable<T> o) => await PUTAsync<IEnumerable<T>>(kTables + tableId + "/updatemany", o);

        #endregion

        #region Delete

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<ApiLog> DeleteAsync(int tableId, string id) => await DELETEAsync<ApiLog>(kTables + tableId + "/delete/" + id, string.Empty);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiLog>> DeleteAsync(int tableId, IEnumerable<string> ids) => await DELETEAsync<IEnumerable<ApiLog>>(kTables + tableId + "/deletemany", ToParamString(ids.ToArray()));

        #endregion

        #endregion

        /// <summary>
        /// Вызывает WebHook
        /// </summary>
        /// <param name="id">Идентификатор хука</param>
        /// <param name="key">Секретный ключ</param>
        /// <param name="content">Данные, которые будут переданы в хук</param>
        /// <param name="contentType">Content-Type, default application/json</param>
        /// <returns></returns>
        public async Task WebHookAsync(string id, string key, string content, string contentType = kMediaTypeHeader) => await POSTAsync(kWebHooks + "/" + id + "/" + key, content, contentType);


    }
}
