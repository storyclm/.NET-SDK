using Shared;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.IoT.Test
{
    public class Test
    {
        const int ITEMSCOUNT = 100;

        Settings _settings => Settings.Get();

        IoTParameters _parameters => new IoTParameters(_settings.IoTHub, _settings.IoTKey, _settings.IoTSecret);

        byte[] Json => File.ReadAllBytes("json.json");
        byte[] Psd => File.ReadAllBytes("psd.psd");

        IEnumerable<string> _items => Enumerable.Range(1, ITEMSCOUNT).Select(t => Guid.NewGuid().ToString("N")).ToArray();

        IDictionary<string, string> _meta = new Dictionary<string, string>
        {
            ["DeviceId"] = "6DCAD6B6E6134A33BAE33B27D6C8C5E5",
            ["UserId"] = "8C21115C-41EB-4428-9215-2367F34BA0DD",
            ["Date"] = DateTime.Now.ToShortTimeString()
        };

        static IEnumerable<Message> GetMessages(IoTParameters parameters, IEnumerable<string> items, string continuationToken)
        {
            List<Message> messages = new List<Message>();
            foreach (var t in new SCLM().GetFeed(parameters, continuationToken))
                messages.AddRange(t.Messages.Where(s => items.Contains(s.Metadata.ContainsKey("item") ? s.Metadata["item"] : null)));
            return messages;
        }

        async Task<Message> Pub(string item, byte[] body)
        {
            return await (new SCLM()).Publish(_parameters, body, new Dictionary<string, string>()
            {
                ["item"] = item
            });
        }

        [Fact]
        public void Sections()
        {
            DateTimeOffset date = DateTimeOffset.UtcNow;

            // текущий год / текущий месяц / текущий день / текущий час
            Assert.Equal($"{date.Year}/{date.Month}/{date.Day}/{date.Hour}/", $"{new HourSection().Hour(date.Hour)}");

            // текущий год / текущий месяц / текущий день / заданный час
            Assert.Equal($"{date.Year}/{date.Month}/{date.Day}/{23}/", $"{new HourSection().Hour(23)}");

            // текущий год / текущий месяц / заданный день / заданный час
            Assert.Equal($"{date.Year}/{date.Month}/{10}/{23}/", $"{new DaySection().Day(10).Hour(23)}");

            // текущий год / заданный месяц / заданный день / заданный час
            Assert.Equal($"{date.Year}/{5}/{10}/{23}/", $"{new MonthSection().Month(5).Day(10).Hour(23)}");

            // заданный год / заданный месяц / заданный день / заданный час
            Assert.Equal($"{2018}/{5}/{10}/{23}/", $"{new YearSection().Year(2018).Month(5).Day(10).Hour(23)}");

            //--------------------------------------------------------------------------------------------------------------------

            // текущий год / текущий месяц / заданный день /
            Assert.Equal($"{date.Year}/{date.Month}/{10}/", $"{new DaySection().Day(10)}"); // собираетс за весь день

            // текущий год / заданный месяц 
            Assert.Equal($"{date.Year}/{5}/", $"{new MonthSection().Month(5)}"); // собирает за весь месяц

            // заданный год /
            Assert.Equal($"{2018}/", $"{new YearSection().Year(2018)}"); // собирает за весь год
        }

        public string GetHash(byte[] message)
        {
            using (SHA512 sha512 = SHA512.Create())
            using (MemoryStream stream = new MemoryStream(message))
            {
                byte[] buffer = new byte[8 * 1024];
                int lenght = 0;
                while ((lenght = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sha512.TransformBlock(buffer, 0, lenght, null, 0);
                }
                sha512.TransformFinalBlock(buffer, 0, 0);
                return $"base64;sha512;{Convert.ToBase64String(sha512.Hash)}";
            }
        }

        async Task Publish(byte[] message)
        {
            string filename = "test.bin";
            try
            {
                string hash = GetHash(message);
                SCLM sclm = new SCLM();
                var result = await sclm.Publish(_parameters, message, _meta);
                Assert.NotNull(result);
                Assert.Equal(message.Length, result.Lenght);
                Assert.Equal(hash, result.Hash);
                using (var file = File.OpenWrite(filename))
                {
                    await result.Save(file);
                }
                Assert.Equal(hash, GetHash(File.ReadAllBytes(filename)));
            }
            catch
            {
                throw;
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        [Fact]
        public async Task PublishMessage()
        {
            byte[] message = Json;
            await Publish(message);

            SCLM sclm = new SCLM();
            var result = await sclm.Publish(_parameters, null, _meta);
            Assert.Null(result.Hash);
            Assert.Equal(0, result.Lenght);

            message = Psd;
            await Publish(message);
        }


        [Fact]
        public async Task PublishMany()
        {
            DateTimeOffset continuationToken = DateTimeOffset.UtcNow;
            var items = _items;
            for (int i = 0; i < items.Count(); i++)
                await Pub(items.ElementAt(i), Json);

            var messages = GetMessages(_parameters, items, continuationToken.Ticks.ToString());

            Assert.Equal(ITEMSCOUNT, messages.Count());
            Assert.True(messages.Where(t => items.Contains(t.Metadata["item"])).Select(t => t.Metadata["item"]).Intersect(items).Count() == items.Count());

            foreach (var t in messages)
                await t.Delete();

            messages = GetMessages(_parameters, items, continuationToken.Ticks.ToString());

            Assert.Empty(messages);
        }

    }
}
