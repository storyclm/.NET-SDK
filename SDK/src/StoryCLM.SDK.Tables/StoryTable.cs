using StoryCLM.SDK.Common.Pumper;
using StoryCLM.SDK.Extensions;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Tables
{
    public class StoryTable<T> where T : class
    {
        internal SCLM _sclm;

        internal StoryTable() { }

        private string ToBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        Uri GetUri(string query) =>
            new Uri($"{_sclm.GetEndpoint("api")}{TablesExtensions.Version}/{TablesExtensions.Path}/{query}", UriKind.Absolute);


        #region Base

        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<StoryTableSchema> Schema { get; set; }

        public DateTime Created { get; set; }

        #endregion



        #region Count

        /// <summary>
        /// Таблицы.
        /// Колличество записей в таблице.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <returns></returns>
        public async Task<long> CountAsync(string query = null, CancellationToken token = default(CancellationToken)) =>
            (await _sclm.GETAsync<StoryCount>(GetUri(Id + "/count" + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), token))
            .Count;


        /// <summary>
        /// Таблицы.
        /// Колличество записей лога таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка. Необязательный параметр.</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(DateTime? date = null, CancellationToken token = default(CancellationToken)) =>
            (await _sclm.GETAsync<StoryCount>(GetUri(Id + "/logcount" + (date == null ? string.Empty : $"?date={new DateTimeOffset(date.Value).ToUnixTimeSeconds()}")), token))
            .Count;

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
        public async Task<T> FindAsync(string id)
        {
            try
            {
                return await _sclm.GETAsync<T>(GetUri(Id + "/findbyid/" + id), default(CancellationToken));
            }
            catch (HttpCommandException ex)
            {
                if (ex.Code == 404) return null;
                throw;
            }
        }

        /// <summary>
        /// Таблицы.
        /// Найти записи в таблице по списку идентификаторов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync(IEnumerable<string> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null || ids.Count() == 0) return Enumerable.Empty<T>();
            ConcurrentBag<T> result = new ConcurrentBag<T>();
            ParallelPumper<KeyValuePair<int, IEnumerable<string>>> pumper = new ParallelPumper<KeyValuePair<int, IEnumerable<string>>>(await ids.GetPages(), ServicePointManager.DefaultConnectionLimit);
            pumper.Handler = async (page) => 
            {
                token.ThrowIfCancellationRequested();
                var res = await _sclm.GETAsync<IEnumerable<T>>(GetUri(Id + "/findbyids" + page.Value.ToIdsQueryArray()), token);
                foreach (var t in res)
                    result.Add(t);
            };
            pumper.Progress = async (p) => { };
            await pumper.Pump();
            return result.ToList();
        }

        /// <summary>
        /// Загружает все данные таблицы в коллекцию.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> PullAsync(CancellationToken token = default(CancellationToken))
        {
            SequentialPumper<T> acc = new SequentialPumper<T>();
            return await acc.Pull(async (skip, take) => await FindAsync(skip: skip, take: take, token: token));
        }

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
        public async Task<IEnumerable<T>> FindAsync(string query = null,
            string sortfield = null,
            int? sort = null,
            int skip = 0,
            int take = 100,
            CancellationToken token = default(CancellationToken))
        {
            string resource = Id + $"/find?skip={skip}&take={take}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<IEnumerable<T>>(GetUri(resource), token);
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
        public async Task<IEnumerable<StoryLogTable>> LogAsync(DateTime? date = null,
            int skip = 0,
            int take = 100,
            CancellationToken token = default(CancellationToken)) =>
            await _sclm.GETAsync<IEnumerable<StoryLogTable>>(GetUri(Id + $"/log?skip={skip}&take={take}" +
                (date == null ? string.Empty : $"&date={new DateTimeOffset(date.Value).ToUnixTimeSeconds()}")), token);

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
        public async Task<T> InsertAsync(T o, CancellationToken token = default(CancellationToken)) =>
            await _sclm.POSTAsync<T>(GetUri(Id + "/insert"), o, token);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> o, CancellationToken token = default(CancellationToken))
        {
            if (o == null || o.Count() == 0) return Enumerable.Empty<T>();
            ConcurrentBag<T> result = new ConcurrentBag<T>();
            ParallelPumper<KeyValuePair<int, IEnumerable<T>>> pumper = new ParallelPumper<KeyValuePair<int, IEnumerable<T>>>(await o.GetPages(100), ServicePointManager.DefaultConnectionLimit);
            pumper.Handler = async (page) =>
            {
                token.ThrowIfCancellationRequested();
                var res = await _sclm.POSTAsync<IEnumerable<T>>(GetUri(Id + "/insertmany"), page.Value, token); ;
                foreach (var t in res)
                    result.Add(t);
            };
            pumper.Progress = async (p) => { };
            await pumper.Pump();
            return result.ToList();
        }

        #endregion

        #region Update

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T o, CancellationToken token = default(CancellationToken)) =>
            await _sclm.PUTAsync<T>(GetUri(Id + "/update"), o, token);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> o, CancellationToken token = default(CancellationToken))
        {
            if (o == null || o.Count() == 0) return Enumerable.Empty<T>();
            ConcurrentBag<T> result = new ConcurrentBag<T>();
            ParallelPumper<T> pumper = new ParallelPumper<T>(o, ServicePointManager.DefaultConnectionLimit);
            pumper.Handler = async (e) =>
            {
                token.ThrowIfCancellationRequested();
                result.Add(await UpdateAsync(e, token));
            };
            pumper.Progress = async (p) => { };
            await pumper.Pump();
            return result.ToList();
        }

        #endregion

        #region Delete

        /// <summary>
        /// Удаляет запись в таблице по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync(string id, CancellationToken token = default(CancellationToken)) =>
            await _sclm.DELETEAsync<T>(GetUri(Id + "/delete/" + id), token);

        /// <summary>
        /// Удаляет записи в таблице по списку идентификаторов.
        /// </summary>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<string> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null || ids.Count() == 0) return Enumerable.Empty<T>();
            ConcurrentBag<T> result = new ConcurrentBag<T>();
            ParallelPumper<KeyValuePair<int, IEnumerable<string>>> pumper = new ParallelPumper<KeyValuePair<int, IEnumerable<string>>>(await ids.GetPages(50), ServicePointManager.DefaultConnectionLimit);
            pumper.Handler = async (page) =>
            {
                token.ThrowIfCancellationRequested();
                var res = await _sclm.DELETEAsync<IEnumerable<T>>(GetUri(Id + "/deletemany" + page.Value.ToIdsQueryArray()), token);
                foreach (var t in res)
                    result.Add(t);
            };
            pumper.Progress = async (p) => { };
            await pumper.Pump();
            return result.ToList();
        }

        /// <summary>
        /// Удаляет все записи таблицы.
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync(CancellationToken token = default(CancellationToken))
        {
            await new SequentialPumper<T>().Pump(
                async (skip, take) => await FindAsync(take: take, token: token),
                async (page) => await DeleteAsync(page.Select(t => t.GetType().GetProperty("_id").GetValue(t, null).ToString())));
        }

        #endregion

        #region Aggregation

        private class AggregationResult<M>
        {
            public M Result { get; set; }
        }

        public async Task<T> MaxAsync<T>(string field, string query = null, CancellationToken token = default(CancellationToken))
        {
            return (await _sclm.GETAsync<AggregationResult<T>>(GetUri(Id + "/max/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), token)).Result;
        }

        public async Task<M> MinAsync<M>(string field, string query = null, CancellationToken token = default(CancellationToken))
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(GetUri(Id + "/min/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), token)).Result;
        }

        public async Task<M> SumAsync<M>(string field, string query = null, CancellationToken token = default(CancellationToken))
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(GetUri(Id + "/sum/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), token)).Result;
        }

        public async Task<double> AvgAsync(string field, string query = null, CancellationToken token = default(CancellationToken))
        {
            return (await _sclm.GETAsync<AggregationResult<double>>(GetUri(Id + "/avg/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), token)).Result;
        }

        private async Task<T> FLAsync(bool first,
            string query = null,
            string sortfield = null,
            int? sort = null,
            CancellationToken token = default(CancellationToken))
        {
            var q = query == null && sortfield == null && sort == null ? "" : "?";
            var f = first ? "first" : "last";
            string resource = Id + $"/{f}{q}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<T>(GetUri(resource), token);
        }

        public async Task<T> FirstAsync(string query = null,
            string sortfield = null,
            int? sort = null,
            CancellationToken token = default(CancellationToken)) =>
            await FLAsync(true, query: query, sortfield: sortfield, sort: sort, token: token);

        public async Task<T> LastAsync(string query = null,
            string sortfield = null,
            int? sort = null,
            CancellationToken token = default(CancellationToken)) =>
            await FLAsync(false, query: query, sortfield: sortfield, sort: sort, token: token);


        #endregion

    }
}
