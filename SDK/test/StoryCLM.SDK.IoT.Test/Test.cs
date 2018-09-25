using Shared;
using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.IoT.Test
{
    public class Test
    {
        static Settings _settings => Settings.Get();

        static IoTParameters CommandParameters => new IoTParameters(_settings.CommandsKey, _settings.CommandSecret);
        static IoTParameters EventParameters => new IoTParameters(_settings.EventsKey, _settings.EventSecret);
        static IoTParameters DataParameters => new IoTParameters(_settings.DataKey, _settings.DataSecret);

        static byte[] CommandBody => File.ReadAllBytes("json.json");

        static IEnumerable<string> Items => Enumerable.Range(1, 100).Select(t => Guid.NewGuid().ToString("N")).ToArray();

        static IEnumerable<Message> GetMessages(IoTParameters parameters, IEnumerable<string> items)
        {
            List<Message> messages = new List<Message>();
            foreach (var t in new SCLM().GetFeed(parameters, section: new HourSection().Hour(DateTimeOffset.UtcNow.Hour)))
                messages.AddRange(t.Messages.Where(s => items.Contains(s.Meta["item"])));
            return messages;
        }

        static async Task<Message> SendCommand(string item, byte[] body)
        {
            using (MemoryStream stream = new MemoryStream(body))
            {
                return await (new SCLM()).PublishCommand(CommandParameters, stream, new Dictionary<string, string>()
                {
                    ["item"] = item
                });
            }
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

        [Fact]
        public async Task PublishCommands()
        {
            var items = Items;
            for (int i = 0; i < items.Count(); i++)
                await SendCommand(items.ElementAt(i), CommandBody);

            var messages = GetMessages(CommandParameters, items);

            Assert.Equal(100, messages.Count());
            Assert.True(messages.Where(t => items.Contains(t.Meta["item"])).Select(t => t.Meta["item"]).Intersect(items).Count() == items.Count());

            foreach (var t in messages)
                await t.Delete();

            messages = GetMessages(CommandParameters, items);

            Assert.Empty(messages);
        }

    }
}
