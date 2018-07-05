/*!
* StoryCLM.SDK Library v2.3.0
* Copyright(c) 2018, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public class SCLM
    {
        const string _api = "api";
        const string _auth = "auth";
        const string _myo = "myo";

        const string _post = "POST";
        const string _put = "PUT";
        const string _get = "GET";
        const string _delete = "DELETE";
        const string _patch = "PATCH";

        static readonly HttpClient _httpClient;

        ILogger _logger;

        static SCLM()
        {
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
            });
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.ConnectionClose = true;
        }

        public SCLM(ILogger logger = null)
        {
            SetEndpoint(_api, "https://api.storyclm.com");
            SetEndpoint(_auth, "https://auth.storyclm.com");
            SetEndpoint(_myo, "https://myosotis.storyclm.com");
            _logger = logger;
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="retryPolicy"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public virtual async Task ExecuteHttpCommand(IHttpCommand command,
            IRetryPolicy retryPolicy,
            CancellationToken cancellationToken,
            ILogger logger = null)
        {
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                if (command == null) return;
                bool retry = retryPolicy == null ? false : retryPolicy.RetriesCount > 0;
                TimeSpan retryInterval = retryPolicy?.Interval ?? TimeSpan.Zero;
                int retryCount = 1;
                do
                {
                    try
                    {
                        DateTime now = DateTime.Now;
                        try // ловим таймаут
                        {
                            cancellationTokenSource.CancelAfter(command.ExpiryTime != TimeSpan.Zero ? (int)command.ExpiryTime.TotalMilliseconds : int.MaxValue);
                            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(command.Method), command.Uri);
                            //пропускаем запрос через декораторы
                            await HttpRequestPipeline(command, request, request.Properties, cancellationTokenSource.Token).ConfigureAwait(false);
                            if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                            //пропускаем запрос через обработчик команды
                            await command.OnExecuting(request, cancellationTokenSource.Token).ConfigureAwait(false);
                            if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token).ConfigureAwait(false);
                            if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                            //пропускаем ответ через декораторы
                            await HttpResponsePipeline(command, response, request.Properties, cancellationTokenSource.Token).ConfigureAwait(false);
                            if (cancellationTokenSource.Token.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                            //пропускаем ответ через обработчик команды
                            await command.OnExecuted(response, cancellationTokenSource.Token).ConfigureAwait(false);
                            return;
                        }
                        catch (Exception ex) //таймаут
                        {
                            if (ex is OperationCanceledException
                                && command.ExpiryTime != TimeSpan.Zero
                                && (now + command.ExpiryTime) < DateTime.Now)
                                throw new TimeoutException(string.Empty, ex);

                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        command.Exception = ex;
                        //обрабатываем повтор.
                        if (!retry
                            || retryPolicy == null
                            || retryCount >= retryPolicy.RetriesCount) throw;

                        //необходим ли повтор (повтор по условию).
                        if (retryPolicy.Predicate != null)
                            if (!retryPolicy.Predicate(command.Exception)) throw;

                        //задержка между повторами
                        if (retryInterval > TimeSpan.Zero) // без задержки или с задержкой
                        {
                            if (retryPolicy.ExponentialBackoff) // экспоненциальная задержка
                            {
                                var pow = Math.Pow(2, retryCount - 1);
                                int delay = (int)(retryInterval.TotalMilliseconds * (pow - 1) / 2);
                                await Task.Delay(delay, cancellationTokenSource.Token).ConfigureAwait(false);
                            }
                            else
                                await Task.Delay((int)retryInterval.TotalMilliseconds * retryCount, cancellationTokenSource.Token).ConfigureAwait(false); // линейная задержка
                        }

                        retryCount++;
                    }
                }
                while (retry);
            }
        }

        #region Endpoints

        readonly IDictionary<string, Uri> _endpoints = new Dictionary<string, Uri>();

        public void SetEndpoint(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri)) throw new ArgumentException(nameof(value));
            _endpoints[name] = uri;
        }

        public Uri GetEndpoint(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            return _endpoints[name];
        }

        #endregion

        #region Decorators

        /// <summary>
        /// Конеыеер запроса.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="request"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        async Task HttpRequestPipeline(IHttpCommand command, HttpRequestMessage request, IDictionary<string, object> parameters, CancellationToken token)
        {
            if (_httpDecorators == null
                || _httpDecorators.Count() == 0
                || request == null) return;

            foreach (var t in _httpDecorators)
            {
                if (token.IsCancellationRequested) return;
                await t.Value.OnExecuting(new HttpPiplineRequest
                {
                    CancellationToken = token,
                    Command = command,
                    Executor = this,
                    Parameters = parameters,
                    Request = request
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Коневеер ответа
        /// </summary>
        /// <param name="command"></param>
        /// <param name="response"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        async Task HttpResponsePipeline(IHttpCommand command, HttpResponseMessage response, IDictionary<string, object> parameters, CancellationToken token)
        {
            if (_httpDecorators == null
                || _httpDecorators.Count() == 0
                || response == null) return;

            foreach (var t in _httpDecorators)
            {
                if (token.IsCancellationRequested) return;
                await t.Value.OnExecuted(new HttpPiplineResponse
                {
                    CancellationToken = token,
                    Command = command,
                    Executor = this,
                    Parameters = parameters,
                    Response = response
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Набор декораторов.
        /// </summary>
        IDictionary<Type, IHttpPiplineDecorator> _httpDecorators = new Dictionary<Type, IHttpPiplineDecorator>();

        public void AddHttpDecorator<T>(T decorator) where T : class, IHttpPiplineDecorator =>
            _httpDecorators[decorator.GetType()] = decorator;

        public T GetHttpDecorator<T>() where T : class, IHttpPiplineDecorator
        {
            if (!_httpDecorators.ContainsKey(typeof(T))) return default(T);
            return _httpDecorators[typeof(T)] as T;
        }

        public void RemoveHttpDecoratot<T>() where T : class, IHttpPiplineDecorator
        {
            if (!_httpDecorators.ContainsKey(typeof(T))) return;
            _httpDecorators.Remove(typeof(T));
        }
        
        #endregion

        #region CRUD

        /// <summary>
        /// Список исключений при которых необходимо повторно выполнить команду.
        /// </summary>
        List<Type> exceptionsForPetry = new List<Type>() {
                    typeof(HttpRequestException),
                    typeof(TimeoutException),
                    typeof(IOException),
                    typeof(EndOfStreamException),
                    typeof(InvalidDataException),
                    typeof(InvalidOperationException),
                    typeof(TaskCanceledException)
                };


        bool AllowableHttErrorCodesForRetry(Exception ex, IEnumerable<int> codes)
        {
            if (ex == null) return false;
            if (exceptionsForPetry.Any(t => ex.GetType() == t)) return true;

            HttpCommandException commandException = ex as HttpCommandException;
            if (commandException == null 
                || codes == null 
                || codes.Count() == 0) return false;

            return codes.Any(t => t == commandException.Code);
        }

        IRetryPolicy CommandPolicy => 
            new BackendRetryPolicy()
            {
                Predicate = (ex) => AllowableHttErrorCodesForRetry(ex, new int[]
                {
                    502,
                    503,
                    504,
                    509,
                    520,
                    521
                })
            };

        IRetryPolicy QueryPolicy =>
           new BackendRetryPolicy()
           {
               Predicate = (ex) => AllowableHttErrorCodesForRetry(ex, new int[]
               {
                    500,
                    502,
                    503,
                    504,
                    509,
                    520,
                    521
               })
           };


        async Task<T> BackendCommand<T>(string method, Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
        {
            using (BackendCommand<T> command = new BackendCommand<T>(method, uri))
            {
                command.Data = o;
                await ExecuteHttpCommand(command, retryPolicy ?? CommandPolicy, cancellationToken, _logger);
                if (command.Exception != null) throw command.Exception;
                return command.Result;
            }
        }

        public async Task<T> POSTAsync<T>(Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_post, uri, o, cancellationToken, retryPolicy);

        public async Task<T> PUTAsync<T>(Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_put, uri, o, cancellationToken, retryPolicy);

        public async Task<T> PATCHAsync<T>(Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_patch, uri, o, cancellationToken, retryPolicy);

        public async Task<T> GETAsync<T>(Uri uri, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_get, uri, null, cancellationToken, retryPolicy ?? QueryPolicy);

        public async Task<T> DELETEAsync<T>(Uri uri, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_delete, uri, null, cancellationToken, retryPolicy);

        #endregion


    }
}
