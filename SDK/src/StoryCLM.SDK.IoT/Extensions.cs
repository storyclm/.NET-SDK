using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public static class Extensions
    {
        const string KEY = "A0523300-9A1F-4B59-8E58-E820061E3BB5";
        const string SECRET = "3C4AD956-2EF2-4B93-8DEC-F5F92EB8C974";

        public static async Task<Message> PublishCommand(this SCLM sclm, Stream data, IDictionary<string, string> meta = null)
        {
            var command = new PublishCommand(KEY, SECRET, data);
            command.Endpoint = new Uri("https://localhost/publish");

            if (meta != null)
                foreach (var t in meta)
                    command[t.Key] = t.Value;

            await sclm.ExecuteHttpCommand(command, sclm.HttpQueryPolicy);

            if (command.Exception != null)
                throw command.Exception;

            return command.Result;
        }
    }
}
