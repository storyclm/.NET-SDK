using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
   public class StoryTable<T> where T : class
    {
        internal SCLM _sclm;

        internal StoryTable()
        {
        }

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
            (await _sclm.GETAsync<StoryCount>(_sclm.kTables + Id + "/count", string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Count;


        /// <summary>
        /// Таблицы.
        /// Колличество записей лога таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="date">Дата, после которой будет произведена выборка. Необязательный параметр.</param>
        /// <returns></returns>
        public async Task<long> LogCountAsync(DateTime? date = null) =>
            (await _sclm.GETAsync<StoryCount>(_sclm.kTables + Id + "/logcount", date == null ? string.Empty : $"?date={ConvertToUnixTime(date.Value).ToString()}")).Count;

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
        public async Task<T> FindAsync(string id) =>
            await _sclm.GETAsync<T>(_sclm.kTables + Id + "/findbyid/" + id, string.Empty);

        /// <summary>
        /// Таблицы.
        /// Найти записи в таблице по списку идентификаторов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync(IEnumerable<string> ids) =>
            await _sclm.GETAsync<IEnumerable<T>>(_sclm.kTables + Id + "/findbyids", ToParamString(ids.ToArray()));

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
            string resource = _sclm.kTables + Id + $"/find?skip={skip}&take={take}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<IEnumerable<T>>(resource, string.Empty);
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
        public async Task<IEnumerable<ApiLog>> LogAsync(DateTime? date = null, int skip = 0, int take = 100) =>
            await _sclm.GETAsync<IEnumerable<ApiLog>>(_sclm.kTables + Id + $"/log?skip={skip}&take={take}" +
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
        public async Task<T> InsertAsync(T o) =>
            await _sclm.POSTAsync<T>(_sclm.kTables + Id + "/insert", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> o) =>
            await _sclm.POSTAsync<IEnumerable<T>>(_sclm.kTables + Id + "/insertmany", o);

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
            await _sclm.PUTAsync<T>(_sclm.kTables + Id + "/update", o);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="o">Сущность</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> o) =>
            await _sclm.PUTAsync<IEnumerable<T>>(_sclm.kTables + Id + "/updatemany", o);

        #endregion

        #region Delete

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync(string id) =>
            await _sclm.DELETEAsync<T>(_sclm.kTables + Id + "/delete/" + id, string.Empty);

        /// <summary>
        /// Таблицы.
        /// </summary>
        /// <param name="tableId">Идентификатор таблицы</param>
        /// <param name="ids">Список идентификаторов записей в таблице</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<string> ids) =>
            await _sclm.DELETEAsync<IEnumerable<T>>(_sclm.kTables + Id + "/deletemany", ToParamString(ids.ToArray()));

        #endregion

        #region Aggregation

        private class AggregationResult<M>
        {
            public M Result { get; set; }
        }

        public async Task<T> MaxAsync<T>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<T>>(_sclm.kTables + Id + "/max/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<M> MinAsync<M>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(_sclm.kTables + Id + "/min/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<M> SumAsync<M>(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<M>>(_sclm.kTables + Id + "/sum/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        public async Task<double> AvgAsync(string field, string query = null)
        {
            return (await _sclm.GETAsync<AggregationResult<double>>(_sclm.kTables + Id + "/avg/" + field, string.IsNullOrEmpty(query) ? string.Empty : $"?query={query}")).Result;
        }

        private async Task<T> FLAsync(bool first, string query = null, string sortfield = null, int? sort = null)
        {
            var q = query == null && sortfield == null && sort == null ? "" : "?";
            var f = first ? "first" : "last";
            string resource = _sclm.kTables + Id + $"/{f}{q}"
                + (sort == null ? "" : $"&sort={sort}")
                + (string.IsNullOrEmpty(sortfield) ? "" : $"&sortfield={sortfield}")
                + (string.IsNullOrEmpty(query) ? "" : $"&query={query}");
            return await _sclm.GETAsync<T>(resource, string.Empty);
        }

        public async Task<T> FirstAsync(string query = null, string sortfield = null, int? sort = null) =>
            await FLAsync(true, query: query, sortfield: sortfield, sort: sort);

        public async Task<T> LastAsync(string query = null, string sortfield = null, int? sort = null) =>
            await FLAsync(false, query: query, sortfield: sortfield, sort: sort);


        #endregion

    }
}
