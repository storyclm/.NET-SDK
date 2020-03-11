using System;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public class StoryKPIPEvent : KPIBase
    {
        public StoryKPIPEvent()
        {
            Hours = new KPIPRawData[24];
        }

        public KPIPRawData Total { get; set; }

        public KPIPRawData[] Hours { get; }
    }
}
