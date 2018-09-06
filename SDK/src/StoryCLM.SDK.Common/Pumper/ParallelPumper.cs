using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Common.Pumper
{
    public class ParallelPumper<T>
    {
        const int DELAY = 10;
        const int MAX_ACTIVE_WORKERS = 1;

        int _activeWorkers;
        int _maxActiveWorkers;
        int _delay;
        readonly ConcurrentQueue<T> _queue;
        readonly List<Task> _activeTasks;

        public Func<T, Task> Handler { get; set; }

        public Func<int, int, Exception, bool> Predicate { get; set; }

        public Func<ParallelPumperProggess<T>, Task> Progress { get; set; }

        public ParallelPumper(IEnumerable<T> items, int maxActiveWorkers = MAX_ACTIVE_WORKERS, int delay = DELAY)
        {
            _activeTasks = new List<Task>();
            _queue = new ConcurrentQueue<T>();

            if (items != null && items.Count() > 0)
                foreach (var t in items) Enqueue(t);

            _maxActiveWorkers = (maxActiveWorkers < 1) ? MAX_ACTIVE_WORKERS : maxActiveWorkers;
            _delay = (delay < 1) ? DELAY : delay;
        }

        #region Queue

        public void Enqueue(T item)
        {
            if (item == null) return;
            _queue.Enqueue(item);
        }

        T Dequeue()
        {
            T bar;
            if (!_queue.TryDequeue(out bar))
                throw new InvalidOperationException("Dequeue error.");
            return bar;
        }

        #endregion

        void IncrementActiveWorkers() => Interlocked.Increment(ref _activeWorkers);
        void DecrementActiveWorkers() => Interlocked.Decrement(ref _activeWorkers);

        public async Task Pump()
        {
            if (Handler == null) throw new ArgumentNullException(nameof(Handler));
            do
            {
                try
                {
                    if (_activeWorkers < _maxActiveWorkers)
                    {
                        int diff = _maxActiveWorkers - _activeWorkers;
                        for (int i = 0; i < diff; i++)
                        {
                            if (_queue.IsEmpty) break;
                            var item = Dequeue();
                            _activeTasks.Add(Task.Run(() => Handler(item).Wait()));
                            IncrementActiveWorkers();
                            await Progress?.Invoke(new ParallelPumperProggess<T> { ActiveWorkers = _activeWorkers, Queue = _queue.Count });
                        }
                    }

                    for (int i = 0; i < _activeWorkers; i++)
                    {
                        Task task = _activeTasks.ElementAt(i);
                        if (task.IsCompleted)
                        {
                            _activeTasks.Remove(task);
                            DecrementActiveWorkers();
                            task = null;
                            await Progress?.Invoke(new ParallelPumperProggess<T> { ActiveWorkers = _activeWorkers, Queue = _queue.Count });
                        }
                    }

                    if (Predicate == null)
                    {
                        if (_activeWorkers == 0 && _queue.IsEmpty) return;
                    }
                    else
                    {
                        if (!Predicate(_queue.Count, _activeWorkers, null)) return;
                    }
                }
                catch (Exception ex)
                {
                    if (Predicate != null)
                    {
                        if (!Predicate(_queue.Count, _activeWorkers, ex)) return;
                    }
                    else throw;
                }
                finally
                {
                    await Task.Delay(_delay);
                }
            }
            while (true);
        }

    }
}
