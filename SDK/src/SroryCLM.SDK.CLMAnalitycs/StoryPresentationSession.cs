using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SroryCLM.SDK.CLMAnalitycs
{
    public class StoryPresentationSession
    {
        /// <summary>
        /// Идентификатор сессии.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Идентификатор презентации.
        /// </summary>
        public int PresentationId { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Завершенная демонстрация.
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// Иднетификатор пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Широта места проведения демонстрации.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Долгота места проведения демонстрации.
        /// </summary>
        public string Longtitude { get; set; }

        /// <summary>
        /// Дата демонстрации. Считается от начала демонстрации.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Продолжительность демонстрации в секундах.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Адрес, где производилась демонстрация.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Колличество слайдов которые были показаны во время демонстрации.
        /// </summary>
        public int SlidesCount { get; set; }

        /// <summary>
        /// Часой пояс.
        /// </summary>
        public int TimeZoneOffset { get; set; }
    }
}
