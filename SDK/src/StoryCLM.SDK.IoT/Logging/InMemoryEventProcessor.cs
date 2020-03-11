using Breffi.Story.Common.Worker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT.Logging
{
    class WorkerItem
    {
        public IDictionary<string, string> Headers { get; set; }

        public byte[] Body { get; set; }
    }

    public class InMemoryEventProcessor : IDisposable
    {
        readonly IoTParameters _parameters;
        readonly CancellationTokenSource _cts;
        readonly Worker<WorkerItem> _worker;
        readonly SCLM _sclm = new SCLM();

        public InMemoryEventProcessor(IoTParameters parameters, 
            CancellationToken ctoken = default(CancellationToken),
            int maxParallelHandlers = 1)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ctoken);
            _worker = new Worker<WorkerItem>(cancellationToken: _cts.Token, maxActiveWorkers: maxParallelHandlers);
            _worker.Handler = Handler;
        }

        public void Add(byte[] body, IDictionary<string, string> metadata = null)
        {
            if (body == null && (metadata == null || metadata.Count == 0)) return;
            _worker.Enqueue(new WorkerItem
            {
                 Body = body,
                 Headers = metadata
            });
        }

        async Task Handler(WorkerItem item)
        {
            try
            {
                await _sclm.Publish(_parameters, item.Body, item.Headers, _cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(InMemoryEventProcessor)}: {ex.Message}");
                throw new ReturnToTheQueueException<WorkerItem>(item);
            }
        }

        public void Dispose() => _cts.Cancel();
    }
}
