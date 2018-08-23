using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public abstract class ResultCommand<TResult> : IotHttpCommand where TResult : class
    {
        public ResultCommand(string key, string secret, Stream data = null) : base(key, secret, data) { }

        public TResult Result { get; set; }
    }
}
