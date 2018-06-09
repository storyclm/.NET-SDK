using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Count
    {
        [Theory]
        [InlineData(0)]
        public async void CountTest(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            long count = await storyTable.CountAsync();
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void CountWithQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            long count = await storyTable.CountAsync("[Gender][eq][false]");
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void LogCount(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            long count = await storyTable.LogCountAsync();
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void LogCountWithDate(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            long count = await storyTable.LogCountAsync(DateTime.Now.AddDays(-5));
            Assert.True(count > 0);
        }

    }
}
