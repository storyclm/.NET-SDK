using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StoryCLM.SDK.IoT.Logging
{
    public class StoryLogger : ILogger
    {
        private readonly string _name;
        private readonly InMemoryEventProcessor _processor;

        public StoryLogger(string name, IoTParameters parameters,
            CancellationToken ctoken = default(CancellationToken),
            int maxParallelHandlers = 1)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            _name = name;
            _processor = new InMemoryEventProcessor(parameters, ctoken, maxParallelHandlers);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _processor.Add(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                Exception = exception
            }, 
            Formatting.Indented)),
            new Dictionary<string, string>
            {
                ["Name"] = _name,
                ["Shema"] = "",
                ["Content-Type"] = "application/json"
            });
        }
    }
}
