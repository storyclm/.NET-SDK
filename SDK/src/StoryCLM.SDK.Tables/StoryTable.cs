using StoryCLM.SDK.Extensions;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<long> CountAsync(string query = null) =>
            (await _sclm.GETAsync<StoryCount>(GetUri(Id + "/count" + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), CancellationToken.None))
            .Count;


        /// <summary>
        /// Таблицы.
        /// Колличество записей лога таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка. Необязательный параметр.</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(DateTime? date = null) =>
            (await _sclm.GETAsync<StoryCount>(GetUri(Id + "/logcount" + (date == null ? string.Empty : $"?date={new DateTimeOffset(date.Value).ToUnixTimeSeconds()}")), CancellationToken.None))
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
               return await _sclm.GETAsync<T>(GetUri(Id + "/findbyid/" + id), CancellationToken.None);
            }
            catch(HttpCommandException ex)
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
        public async Task<IEnumerable<T>> FindAsync(IEnumerable<string> ids) =>
            await _sclm.GETAsync<IEnumerable<T>>(GetUri(Id + "/findbyids" + ids.ToIdsQueryArray()), CancellationToken.None);

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
        public async Task<IEnumerable<T>> FindAsync(string query = null, string sortfield = null, int? sort = null, int skip = 0, int take = 100)
        {
            string resource = Id + $"/find?skip={skip}&take={take}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<IEnumerable<T>>(GetUri(resource), CancellationToken.None);
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
        public async Task<IEnumerable<StoryLogTable>> LogAsync(DateTime? date = null, int skip = 0, int take = 100) =>
            await _sclm.GETAsync<IEnumerable<StoryLogTable>>(GetUri(Id + $"/log?skip={skip}&take={take}" +
                (date == null ? string.Empty : $"&date={new DateTimeOffset(date.Value).ToUnixTimeSeconds()}")), CancellationToken.None);

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
        public async Task<T> InsertAsync(T o) =>
            await _sclm.POSTAsync<T>(GetUri(Id + "/insert"), o, CancellationToken.None);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> o) =>
            await _sclm.POSTAsync<IEnumerable<T>>(GetUri(Id + "/insertmany"), o, CancellationToken.None);

        #endregion

        #region Update

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T o) =>
            await _sclm.PUTAsync<T>(GetUri(Id + "/update"), o, CancellationToken.None);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> o) =>
            await _sclm.PUTAsync<IEnumerable<T>>(GetUri(Id + "/updatemany"), o, CancellationToken.None);

        #endregion

        #region Delete

        /// <summary>
        /// Удаляет запись в таблице по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync(string id) =>
            await _sclm.DELETEAsync<T>(GetUri(Id + "/delete/" + id), CancellationToken.None);

        /// <summary>
        /// Удаляет записи в таблице по списку идентификаторов.
        /// </summary>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<string> ids) =>
            await _sclm.DELETEAsync<IEnumerable<T>>(GetUri(Id + "/deletemany" + ids.ToIdsQueryArray()), CancellationToken.None);

        #endregion

        #region Aggregation

        private class AggregationResult<M>
        {
            public M Result { get; set; }
        }

        public async Task<T> MaxAsync<T>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<T>>(GetUri(Id + "/max/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), CancellationToken.None)).Result;
        }

        public async Task<M> MinAsync<M>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(GetUri(Id + "/min/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), CancellationToken.None)).Result;
        }

        public async Task<M> SumAsync<M>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(GetUri(Id + "/sum/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), CancellationToken.None)).Result;
        }

        public async Task<double> AvgAsync(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<double>>(GetUri(Id + "/avg/" + field + (string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")), CancellationToken.None)).Result;
        }

        private async Task<T> FLAsync(bool first, string query = null, string sortfield = null, int? sort = null)
        {
            var q = query == null && sortfield == null && sort == null ? "" : "?";
            var f = first ? "first" : "last";
            string resource = Id + $"/{f}{q}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<T>(GetUri(resource), CancellationToken.None);
        }

        public async Task<T> FirstAsync(string query = null, string sortfield = null, int? sort = null) =>
            await FLAsync(true, query: query, sortfield: sortfield, sort: sort);

        public async Task<T> LastAsync(string query = null, string sortfield = null, int? sort = null) =>
            await FLAsync(false, query: query, sortfield: sortfield, sort: sort);


        #endregion

    }
}
