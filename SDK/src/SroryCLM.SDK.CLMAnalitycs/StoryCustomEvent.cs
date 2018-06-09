using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SroryCLM.SDK.CLMAnalitycs
{
    /// <summary>
    /// Кастомное событие.
    /// </summary>
    public class StoryCustomEvent
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
        /// Идентификатор пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Ключ записи.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Дата создания на устройстве.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public int TimeZoneOffset { get; set; }
    }
}
