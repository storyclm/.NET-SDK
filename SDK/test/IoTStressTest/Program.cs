using StoryCLM.SDK;
using StoryCLM.SDK.Common.Pumper;
using StoryCLM.SDK.IoT;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace IoTStressTest
{
    class Program
    {
        static IoTParameters CommandParameters => new IoTParameters("A0523300-9A1F-4B59-8E58-E820061E3BB5", "3C4AD956-2EF2-4B93-8DEC-F5F92EB8C974");

        static void Main(string[] args)
        {
            Run().Wait();
            Console.ReadLine();
        }

        static async Task Run()
        {
            await SendCommands();
        }

        static async Task SendCommands()
        {
            var file = File.ReadAllBytes("json.json");
            ParallelPumper<int> extractor = new ParallelPumper<int>(Enumerable.Range(1, 100000), 20);
            extractor.Handler = async (item) =>
            {
                try
                {
                    SCLM sclm = new SCLM();
                    using (MemoryStream stream = new MemoryStream(file))
                    {
                        var result = await sclm.PublishCommand(CommandParameters, stream, new Dictionary<string, string>()
                        {
                            ["id"] = item.ToString(),
                            ["part"] = "1",
                            ["version"] = "0.1.0"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            };
            extractor.Progress = async (progress) =>
            {
                Console.Clear();
                Console.WriteLine($"ActiveWorkers: {progress.ActiveWorkers}, Queue: {progress.Queue}.");
            };
            await extractor.Pump();


        }


    }
}
