using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.Common.Pumper
{
    public class ParallelPumperProggess<T>
    {
        public int ActiveWorkers { get; internal set; }

        public int Queue { get; internal set; }
    }
}
