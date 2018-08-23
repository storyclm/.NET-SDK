using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StoryCLM.SDK.IoT
{
    public abstract class IotHttpCommand : IHttpCommand
    {
        string _key;
        string _secret;

        public IotHttpCommand(string key, string secret, Stream data = null)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _secret = secret ?? throw new ArgumentNullException(nameof(secret));
            Data = data;
        }

        #region Parameters

        NameValueCollection _parameters = HttpUtility.ParseQueryString(string.Empty);

        public virtual void SetParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            _parameters[name] = value;
        }

        public virtual string GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return _parameters[name];
        }

        public virtual void Delete(string name) =>
            _parameters.Remove(name);

        /// <summary>
        /// Уникальный идентификатор устройсва. Необязательный параметр. Может быть затребован настройками хаба.
        /// </summary>
        public virtual string DeviceId
        {
            get => _parameters["did"];

            set
            {
                if (!string.IsNullOrEmpty(value))
                    _parameters["did"] = value;
            }
        }

        /// <summary>
        /// IP адрес устройства или диапазон адресов. Необязательный параметр. Может быть затребован настройками хаба.
        /// </summary>
        public virtual string IP
        {
            get => _parameters["ip"];

            set
            {
                if (!string.IsNullOrEmpty(value))
                    _parameters["ip"] = IP;
            }
        }

        DateTimeOffset? _current;

        /// <summary>
        /// Текущее время по UTC. Текущее время с погрешностью в 5 секунд. Необязательный параметр. Может быть затребован настройками хаба.
        /// </summary>
        public virtual DateTimeOffset? Current
        {
            get => _current;

            set
            {
                if (value != null && value.HasValue)
                    _parameters["ct"] = value.Value.ToUnixTimeMilliseconds().ToString();

                _current = value;
            }
        }

        DateTimeOffset? _expiration;

        /// <summary>
        /// Дата окончания действия подписи по UTC. 
        /// Время жизни сигнатуры. 
        /// Необязательный параметр. Может быть затребован настройками хаба. 
        /// Один и тот же набор параметров с сигнатурой можно использовать для публикации или потребления сообщений неограниченное количество раз если не будет указан этот параметр. 
        /// Если указан параметр Expiration, то сигнатура будет валидна только до даты указанной в параметре. 
        /// После истечение этого времени сервер будет возвращать ошибку. 
        /// Для большей безопасности рекомендуется задавать этот параметр в настройках хаба как обязательный.
        /// </summary>
        public virtual DateTimeOffset? Expiration
        {
            get => _expiration;

            set
            {
                if (value != null && value.HasValue)
                    _parameters["ext"] = value.Value.ToUnixTimeMilliseconds().ToString();

                _expiration = value;
            }
        }


        #endregion

        #region Properties

        public virtual string MediaType { get; set; } = "application/json";

        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        public virtual Uri Endpoint { get; set; } = new Uri("https://iot.storyclm.com/publish");

        public virtual Stream Data { get; set; }

        public virtual string Method { get; set; } = "POST";

        public virtual Uri Uri
        {
            get
            {
                var builder = new UriBuilder(Endpoint);
                _parameters["key"] = _key;
                _parameters["sig"] = GetSignature();
                builder.Query = _parameters.ToString();
                return builder.Uri;
            }

            set => throw new NotImplementedException();

        }

        public virtual TimeSpan ExpiryTime { get; set; } = TimeSpan.FromSeconds(3600);

        public virtual Exception Exception { get; set; }

        #endregion

        public abstract Task OnExecuting(HttpRequestMessage request, CancellationToken token);

        public abstract Task OnExecuted(HttpResponseMessage response, CancellationToken token);

        string GetSignature()
        {
            StringBuilder text = new StringBuilder($"key={_key}");

            if (Expiration != null && Expiration.HasValue)
                text.Append($"ext={Expiration.Value.ToUnixTimeMilliseconds()}");

            if (!string.IsNullOrEmpty(DeviceId))
                text.Append($"did={DeviceId}");

            if (!string.IsNullOrEmpty(IP))
                text.Append($"ip={IP}");

            if (Current != null && Current.HasValue)
                text.Append($"ct={Current.Value.ToUnixTimeMilliseconds()}");

            using (HMACSHA256 encoder = new HMACSHA256(Encoding.UTF8.GetBytes(_secret)))
            {
                return BitConverter.ToString(encoder.ComputeHash(Encoding.UTF8.GetBytes(text.ToString()))).Replace("-", "").ToLower();
            }
        }

        public void Dispose()
        {
            if (Data == null) return;
            Data.Dispose();
        }
    }
}
