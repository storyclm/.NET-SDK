using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK
{
    public class HttpCommandException : Exception
    {
        public HttpCommandException(int code, string message, Exception inner = null) : base(message, inner)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}
