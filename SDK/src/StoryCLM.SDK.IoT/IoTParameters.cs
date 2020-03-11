using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    /// <summary>
    /// Параметры сообщения. Используются при запросе.
    /// </summary>
    public class IoTParameters
    {
        public IoTParameters(string hub, 
            string key, 
            string secret)
        {
            Hub = hub ?? throw new ArgumentNullException(nameof(hub));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        public Uri Endpoint { get; set; } = new Uri("https://iot.storychannels.app");

        public string Hub { get; set; }

        public string Key { get; private set; }

        public string Secret { get; private set; }

        /// <summary>
        /// Уникальный идентификатор устройсва. Необязательный параметр. Может быть затребован настройками хаба.
        /// </summary>
        public virtual string DeviceId { get; set; }

        /// <summary>
        /// Текущее время по UTC. Текущее время с погрешностью в 5 секунд. Необязательный параметр. Может быть затребован настройками хаба.
        /// </summary>
        public bool Current { get; set; }

        /// <summary>
        /// Дата окончания действия сигнатуры по UTC. 
        /// Время жизни сигнатуры. 
        /// Необязательный параметр. Может быть затребован настройками хаба. 
        /// Один и тот же набор параметров с сигнатурой можно использовать для публикации или потребления сообщений неограниченное количество раз, если не будет указан этот параметр. 
        /// Если указан параметр Expiration, то сигнатура будет валидна только до даты указанной в параметре. 
        /// После истечение этого времени сервер будет возвращать ошибку. 
        /// Для большей безопасности рекомендуется задавать этот параметр в настройках хаба как обязательный.
        /// </summary>
        public TimeSpan? Expiration { get; set; }
    }
}
