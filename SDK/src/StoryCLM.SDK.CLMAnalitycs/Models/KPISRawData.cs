namespace StoryCLM.SDK.CLMAnalitycs
{
    public class KPISRawData
    {
        /// <summary>
        /// Количество показов слайда
        /// </summary>
        public int ViewsCount { get; set; }

        /// <summary>
        /// Общая продолжительность показа слайда
        /// </summary>
        public int DurationSum { get; set; }

        /// <summary>
        /// Сумма квадратов продолжительностей показа слайда
        /// </summary>
        public long DurationSquareSum { get; set; }
    }
}
