using System;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public class StoryAnalitycsEvent : IFeedable
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
        /// Иднетификатор пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Дата и время начала демонстрации в тиках. 
        /// </summary>
        public long LocalTicks { get; set; }

        /// <summary>
        /// Дата и время записи в базе в тиках.
        /// </summary>
        public long? Ticks { get; set; } = DateTime.UtcNow.Ticks;

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public int? TimeZone { get; set; }
    }
}
