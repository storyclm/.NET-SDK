namespace StoryCLM.SDK.CLMAnalitycs.Models
{
    /// <summary>
    /// Кастомное событие.
    /// </summary>
    public class StoryCustomEvent : StoryAnalitycsEvent
    {
        public string Id { get; set; }
        /// <summary>
        /// Ключ.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}
