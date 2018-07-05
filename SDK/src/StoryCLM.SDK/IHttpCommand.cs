using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public interface IHttpCommand : IDisposable
    {
        string Method { get; set; }

        /// <summary>
        /// Адрес ресурса
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// Таймаут
        /// </summary>
        TimeSpan ExpiryTime { get; set; }

        /// <summary>
        /// Исплючение, возникшее во время выполения.
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Вызывается перед отправкой запроса.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task OnExecuting(HttpRequestMessage request, CancellationToken token);

        /// <summary>
        /// Вызывается перед ответом.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        Task OnExecuted(HttpResponseMessage response, CancellationToken token);

    }
}
