namespace StoryCLM.SDK.CLMAnalitycs.Models
{
    public class StorySlideEvent : StoryAnalitycsEvent
    {
        public string Id { get; set; }

        /// <summary>
        /// Продолжительность в секундах.
        /// </summary>
        public int Duration { get; set; }

        /// <summary> 
        /// Тип навигации.
        /// </summary>
        public string Navigation { get; set; }

        public string SlideName { get; set; }

        public int SlideId { get; set; }
    }
}
