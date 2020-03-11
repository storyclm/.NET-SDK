using Breffi.Story.Common.Worker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT.Logging
{
    public class InMemoryBatchingProcessor<T> : IDisposable
    {
        readonly IoTParameters _parameters;
        readonly CancellationTokenSource _cts;
        readonly BatchWorker<T> _worker;
        readonly SCLM _sclm = new SCLM();

        public InMemoryBatchingProcessor(IoTParameters parameters,
            CancellationToken ctoken = default(CancellationToken),
            int maxBatchSize = 25)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ctoken);
            _worker = new BatchWorker<T>(cancellationToken: _cts.Token, batchSize: maxBatchSize);
            _worker.Handler = Handler;
        }

        async Task Handler(IEnumerable<T> items)
        {
            try
            {
                await _sclm.Publish(_parameters,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(items, Formatting.Indented)),
                    new Dictionary<string, string>
                    {
                        ["Type"] = typeof(T).ToString(),
                        ["Count"] = items.Count().ToString(),
                        ["Content-Type"] = "application/json"
                    }, _cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(InMemoryBatchingProcessor<T>)}: {ex.Message}");
            }
        }

        public void Add(T item) => _worker.Enqueue(item);

        public void Dispose() => _cts.Cancel();
    }
}
