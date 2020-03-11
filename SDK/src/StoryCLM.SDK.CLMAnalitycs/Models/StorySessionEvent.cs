namespace StoryCLM.SDK.CLMAnalitycs.Models
{
    public class StorySessionEvent : StoryAnalitycsEvent
    {
        /// <summary>
        /// Завершенная демонстрация.
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// Широта места проведения демонстрации.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота места проведения демонстрации.
        /// </summary>
        public double? Longtitude { get; set; }

        /// <summary>
        /// Продолжительность демонстрации в секундах.
        /// </summary>
        public int Duration { get; set; }


        public int? CompletionDuration { get; set; }

        /// <summary>
        /// Адрес, где производилась демонстрация.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Колличество слайдов которые были показаны во время демонстрации.
        /// </summary>
        public int SlidesCount { get; set; }
    }
}
