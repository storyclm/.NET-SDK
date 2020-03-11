namespace StoryCLM.SDK.CLMAnalitycs
{
    public class KPIPRawData
    {
        /// <summary>
        /// Количество сессий
        /// </summary>
        public int SessionsCount { get; set; }

        /// <summary>
        /// Количество завершенных сессий
        /// </summary>
        public int CompleteSessionsCount { get; set; }

        /// <summary>
        /// Общая продолжительность сессий презентации
        /// </summary>
        public int DurationSum { get; set; }

        /// <summary>
        /// Сумма квадратов продолжительностей показа сессий
        /// </summary>
        public long DurationSquareSum { get; set; }

        /// <summary>
        /// Общее количество показов слайдов презентации
        /// </summary>
        public int SlidesCount { get; set; }

        /// <summary>
        /// Сумма квадратов количества слайдов за сессию
        /// </summary>
        public long SlidesPerSessionCountSquareSum { get; set; }
    }
}
