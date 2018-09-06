using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public abstract class ResultCommand<TResult> : IotHttpCommand where TResult : class
    {
        public ResultCommand(IoTParameters parameters, Stream data = null) : base(parameters, data) { }

        public TResult Result { get; set; }
    }
}
