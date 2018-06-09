using Shared;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Log
    {
        [Theory]
        [InlineData(0)]
        public async void LogCount(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            IEnumerable<StoryLogTable> logs = await storyTable.LogAsync(DateTime.Now.AddDays(-5), 0, 900);
            Assert.True(logs.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void LogWithDate(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            IEnumerable<StoryLogTable> logs = await storyTable.LogAsync(DateTime.Now.AddDays(-5));
            Assert.True(logs.Count() > 0);

        }

        [Theory]
        [InlineData(0)]
        public async void LogSkipTake(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            IEnumerable<StoryLogTable> logs = await storyTable.LogAsync(skip: 0, take: 900);
            Assert.True(logs.Count() > 0);

        }

        [Theory]
        [InlineData(0)]
        public async void LogAll(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            IEnumerable<StoryLogTable> logs = await storyTable.LogAsync();
            Assert.True(logs.Count() > 0);
        }
    }
}
