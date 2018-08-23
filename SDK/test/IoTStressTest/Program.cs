using Newtonsoft.Json;
using StoryCLM.SDK;
using StoryCLM.SDK.IoT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace IoTStressTest
{
    class Program
    {

        public class Extractor<T>
        {
            /// <summary>
            /// Обработчик
            /// </summary>
            public Action<T> Worker { get; set; }

            /// <summary>
            /// Наблюдатель
            /// </summary>
            public Action<Extractor<T>> Observer { get; set; }

            /// <summary>
            /// Источник
            /// </summary>
            public Func<IEnumerable<T>> Source { get; set; }

            public Extractor(IEnumerable<T> items, int maxActiveWorkers = 10, int delay = 25)
            {
                if (items != null && items.Count() > 0)
                    foreach (var t in items) Enqueue(t);

                _maxActiveWorkers = (maxActiveWorkers < 1) ? 1 : maxActiveWorkers;
                _delay = (delay < 1) ? 25 : delay;
            }

            #region Queue

            ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

            void Enqueue(T task)
            {
                if (task == null) return;
                _queue.Enqueue(task);
            }

            T Dequeue()
            {
                T bar;
                if (!_queue.TryDequeue(out bar)) throw new InvalidOperationException("Dequeue error");
                return bar;
            }

            #endregion


            public static object lockObject = new object();
            int _activeWorkers = 0;
            int _maxActiveWorkers = 0;
            int _delay = 0;

            private void IncrementActiveWorkers() => Interlocked.Increment(ref _activeWorkers);
            private void DecrementActiveWorkers() => Interlocked.Decrement(ref _activeWorkers);

            List<Task> _activeTasks = new List<Task>();

            public void Start()
            {
                if (Worker == null) throw new ArgumentNullException(nameof(Worker));
                if (Source == null) throw new ArgumentNullException(nameof(Source));
                do
                {
                    try
                    {
                        if (_queue.IsEmpty)
                        {
                        }

                        if (_activeWorkers < _maxActiveWorkers)
                        {
                            int diff = _maxActiveWorkers - _activeWorkers;
                            for (int i = 0; i < diff; i++)
                            {
                                if (_queue.IsEmpty) break;
                                var item = Dequeue();
                                Task task = new Task(() =>
                                {
                                    try
                                    {
                                        Worker.Invoke(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"ERRRRRRRRRRRRR: {ex.Message}");
                                    }
                                });
                                _activeTasks.Add(task);
                                IncrementActiveWorkers();
                                task.Start();
                            }
                        }

                        for (int i = 0; i < _activeWorkers; i++)
                        {
                            Task task = _activeTasks.ElementAt(i);
                            if (task.IsCompleted)
                            {
                                Console.WriteLine("Remove");
                                _activeTasks.Remove(task);
                                DecrementActiveWorkers();
                                task = null;
                            }
                        }

                        b = _activeTasks.Count();
                        a = _activeWorkers;
                        if (_activeWorkers == 0 && _queue.IsEmpty) return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                    finally
                    {
                        Thread.Sleep(_delay);
                    }
                }
                while (true);
            }

        }

        static void Main(string[] args)
        {
            
            Timer timer = new Timer((e) =>
            {
                int count = TasksResult.IsEmpty ? 0 : TasksResult.Count();
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                int tasksPerSecond = (currentTime - StartTime).Seconds == 0 ? 0 : count / (currentTime - StartTime).Seconds;
                int average = TasksResult.IsEmpty ? 0 : (int)TasksResult.Average(t => t.Value);
                int errors = TasksErrors.IsEmpty ? 0 : TasksErrors.Count();
                //Console.Clear();
                Console.WriteLine($"b: {b} Tasks: {TaskCount}, workers: {a}, t/s: {tasksPerSecond}, average: {average} errors: {TasksErrors.Count()}");
                StartTime = DateTimeOffset.UtcNow;
                TaskCount += TasksResult.Count();
                TasksResult.Clear();
            }, null, 0, 1000);
            Run().Wait();
        }

        static ConcurrentDictionary<int, long> TasksResult = new ConcurrentDictionary<int, long>();
        static ConcurrentDictionary<int, string> TasksErrors = new ConcurrentDictionary<int, string>();
        static DateTimeOffset StartTime;
        static long TaskCount = 0;

        static int a = 0;
        static int b = 0;

        static async Task Run()
        {
            var file = File.ReadAllBytes("json.json");

            Extractor<int> extractor = new Extractor<int>(Enumerable.Range(1, 100000000), 2);
            extractor.Worker = async (item) => 
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    SCLM sclm = new SCLM();
                    using (MemoryStream data = new MemoryStream(file))
                    {
                        var result = await sclm.PublishCommand(data, new Dictionary<string, string>()
                        {
                            ["id"] = item.ToString(),
                            ["part"] = "1/1",
                            ["version"] = "0.1.0"
                        });
                       // Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: item: {item}, message: {ex.Message}");
                    TasksErrors[item] = ex.Message;
                }
                finally
                {
                    sw.Stop();
                    TasksResult[item] = sw.ElapsedMilliseconds;
                }
            };
            extractor.Source = () => 
            {
                return null;
            };
            extractor.Start();

   
            Console.ReadLine();
        }



    }
}
