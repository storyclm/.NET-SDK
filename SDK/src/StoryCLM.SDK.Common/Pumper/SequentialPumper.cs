using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Common.Pumper
{
    /// <summary>
    /// Насос.
    /// Перекачивает данные из источника порциями и отправляет на обработку.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SequentialPumper<T>
    {
        public SequentialPumper(int buffer = 1000) => BufferLength = buffer;

        public int BufferLength { get; set; } = 1000;

        /// <summary>
        /// Извлечение из источника порциями и объеденение в коллекцию.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Pull(Func<int, int, Task<IEnumerable<T>>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            List<T> result = new List<T>();
            int currentPage = 1;
            while (true)
            {
                var buffer = await source((currentPage - 1) * BufferLength, BufferLength);
                if (buffer == null || buffer.Count() == 0) break;
                result.AddRange(buffer.ToList());
                currentPage++;
            }
            return result;
        }

        /// <summary>
        /// Извлечение из коллекции и обработка порциями.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task Pump(IEnumerable<T> source, Func<IEnumerable<T>, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            long count = source.Count();
            if (count != 0)
            {
                int currentPage = 1;
                while (true)
                {
                    List<T> buffer = source.Skip((currentPage - 1) * BufferLength).Take(BufferLength).ToList();
                    if (buffer.Count() == 0) break;
                    await handler(buffer);
                    currentPage++;
                }
            }
        }

        /// <summary>
        /// Извлечение из источника и обработка порциями.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task Pump(Func<int, int, Task<IEnumerable<T>>> source, Func<IEnumerable<T>, Task> handler)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            int currentPage = 1;
            while (true)
            {
                var buffer = await source((currentPage - 1) * BufferLength, BufferLength);
                if (buffer == null || buffer.Count() == 0) break;
                await handler(buffer);
                currentPage++;
            }
        }
    }
}
