using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    /// <summary>
    /// Декораторы корректируют запрос и ответ перед тем как он будет выполнен или ответ передан на обработку команде.
    /// </summary>
    public interface IHttpPiplineDecorator
    {
        /// <summary>
        /// Вызывается перед отправкой запроса и методом OnExecuting команды.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="request"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task OnExecuting(HttpPiplineRequest context);

        /// <summary>
        /// Вызывается после получения ответа и перед методом OnExecuted команды.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="response"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task OnExecuted(HttpPiplineResponse context);
    }
}
