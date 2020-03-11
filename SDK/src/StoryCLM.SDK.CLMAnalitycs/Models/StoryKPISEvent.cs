using System;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public class StoryKPISEvent : KPIBase
    {
        public string SlideName { get; set; }

        public int SlideId { get; set; }

        public KPISRawData Total { get; set; }

        public KPISRawData[] Hours { get; private set; } = new KPISRawData[24];
    }
}
