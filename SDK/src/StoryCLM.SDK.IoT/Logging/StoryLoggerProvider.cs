using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public class StoryLoggerProvider : ILoggerProvider
    {
        readonly IoTParameters _parameters;

        public StoryLoggerProvider(IoTParameters parameters)
        {
            _parameters = parameters;
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
