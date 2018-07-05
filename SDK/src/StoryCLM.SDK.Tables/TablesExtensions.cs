using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Tables
{
    public static class TablesExtensions
    {
        public static string Version = "v1";
        public static string Path = @"tables";
        const string api = nameof(api);

        /// <summary>
        /// Получить все таблицы клиента.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sclm"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<StoryTable<T>>> GetTablesAsync<T>(this SCLM sclm, int clientId) where T : class, new()
        {
            IEnumerable<StoryTable<T>> result = await sclm.GETAsync<IEnumerable<StoryTable<T>>>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{Path}/{clientId}/tables", UriKind.Absolute), CancellationToken.None);
            foreach (var t in result) t._sclm = sclm;
            return result;
        }

        /// <summary>
        /// Получитьтаблицу клиента по идентификатору.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sclm"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public static async Task<StoryTable<T>> GetTableAsync<T>(this SCLM sclm, int tableId) where T : class, new()
        {
            StoryTable<T> table = await sclm.GETAsync<StoryTable<T>>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{Path}/{tableId}", UriKind.Absolute), CancellationToken.None);
            table._sclm = sclm;
            return table;
        }

        public static async Task<StoryTable<T>> GetTableAsync<T>(this SCLM sclm, string name) where T : class, new()
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name)); 
            StoryTable<T> table = await sclm.GETAsync<StoryTable<T>>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{Path}/{name}", UriKind.Absolute), CancellationToken.None);
            table._sclm = sclm;
            return table;
        }
    }
}
