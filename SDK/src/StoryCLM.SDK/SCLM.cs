/*!
* StoryCLM.SDK Library v2.3.5
* Copyright(c) 2019, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Breffi.Story.Common.Retry;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK
{
    public class SCLM
    {
        const string _api = "api";
        const string _auth = "auth";
        const string _myo = "myo";
        const string _iot = "iot";

        const string _post = "POST";
        const string _put = "PUT";
        const string _get = "GET";
        const string _delete = "DELETE";
        const string _patch = "PATCH";

        const int _defaultConnectionLimit = 10;

        static readonly HttpClient _httpClient;

        ILogger _logger;

        static SCLM()
        {
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
            });
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
        }

        public SCLM(ILogger logger = null)
        {
            SetEndpoint(_api, "https://api.storyclm.com");
            SetEndpoint(_auth, "https://auth.storyclm.com");
            SetEndpoint(_myo, "https://myosotis.storyclm.com");
            SetEndpoint(_iot, "https://iot.storychannels.app");
            _logger = logger;
        }

        public virtual async Task ExecuteHttpCommand(IHttpCommand command,
            IRetryPolicy retryPolicy = null,
            CancellationToken cancellationToken = default(CancellationToken),
            ILogger logger = null)
        {
            if (command == null) return;
            async Task action()
            {
                try
                {
                    using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
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
                            command.Exception = null;
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
                }
                catch (Exception ex)
                {
                    command.Exception = ex;
                    throw;
                }
            }

            if (retryPolicy == null)
            {
                await action();
            }
            else
            {
                IRetry retry = new Retry();
                await retry.Execute(action, retryPolicy, cancellationToken);
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
        /// Список исключений при которых необходимо повторно выполнить код.
        /// </summary>
        public static List<Type> Exceptions { get; } = new List<Type>()
        {
            typeof(IOException),
            typeof(SocketException),
            typeof(HttpRequestException),
            typeof(TimeoutException),
            typeof(EndOfStreamException),
            typeof(InvalidDataException),
            typeof(InvalidOperationException),
            typeof(TaskCanceledException),
            typeof(WarningException),
            typeof(HttpListenerException),
            typeof(ProtocolViolationException),
            typeof(WebException),
            typeof(PingException),
            typeof(NetworkInformationException)
        };

        /// <summary>
        /// Список Http кодов при получении который необходимо выполнить повтор.
        /// </summary>
        public static List<int> HttpsCodes { get; } = new List<int>()
        {
            408,
            417,
            429,
            502,
            503,
            504,
            509,
            520,
            521
        };

        static bool HttpPredicate(Exception ex, IEnumerable<int> codes)
        {
            if (ex == null) return false;
            if (Exceptions.Any(t => ex.GetType() == t)) return true;

            HttpCommandException commandException = ex as HttpCommandException;
            if (commandException == null
                || codes == null
                || codes.Count() == 0) return false;

            return codes.Any(t => t == commandException.Code);
        }

        public IRetryPolicy HttpCommandPolicy =>
             new RetryPolicy()
             {
                 Predicate = (ex) => HttpPredicate(ex, HttpsCodes)
             };

        public IRetryPolicy HttpQueryPolicy =>
            new RetryPolicy()
            {
                Predicate = (ex) => HttpPredicate(ex, new List<int>(HttpsCodes) { 500 })
            };

        async Task<T> BackendCommand<T>(string method, Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
        {
            using (BackendCommand<T> command = new BackendCommand<T>(method, uri))
            {
                command.Data = o;
                await ExecuteHttpCommand(command, retryPolicy ?? HttpQueryPolicy, cancellationToken, _logger);
                if (command.Exception != null) throw command.Exception;
                return command.Result;
            }
        }

        public async Task<T> POSTAsync<T>(Uri uri, object o, CancellationToken cancellationToken = default(CancellationToken), IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_post, uri, o, cancellationToken, retryPolicy ?? HttpCommandPolicy);

        public async Task<T> PUTAsync<T>(Uri uri, object o, CancellationToken cancellationToken = default(CancellationToken), IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_put, uri, o, cancellationToken, retryPolicy);

        public async Task<T> PATCHAsync<T>(Uri uri, object o, CancellationToken cancellationToken = default(CancellationToken), IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_patch, uri, o, cancellationToken, retryPolicy);

        public async Task<T> GETAsync<T>(Uri uri, CancellationToken cancellationToken = default(CancellationToken), IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_get, uri, null, cancellationToken, retryPolicy);

        public async Task<T> DELETEAsync<T>(Uri uri, CancellationToken cancellationToken = default(CancellationToken), IRetryPolicy retryPolicy = null) where T : class
            => await BackendCommand<T>(_delete, uri, null, cancellationToken, retryPolicy);

        #endregion
    }
}
