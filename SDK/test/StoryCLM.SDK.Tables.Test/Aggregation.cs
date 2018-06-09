using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Aggregation
    {
        [Theory]
        [InlineData(0)]
        public async void Max(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolResult = await storyTable.MaxAsync<bool>("Gender");
            var intResult = await storyTable.MaxAsync<int>("Age");
            var stringResult = await storyTable.MaxAsync<string>("Name");
            var datetimeResult = await storyTable.MaxAsync<DateTime>("Created");
            var doubleResult = await storyTable.MaxAsync<double>("Rating");
        }

        [Theory]
        [InlineData(0)]
        public async void MaxQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolQueryResult = await storyTable.MaxAsync<bool>("Gender", "[Gender][eq][false]");
            var intQueryResult = await storyTable.MaxAsync<int>("Age", "[Gender][eq][false]");
            var stringQueryResult = await storyTable.MaxAsync<string>("Name", "[Gender][eq][false]");
            var datetimeQueryResult = await storyTable.MaxAsync<DateTime>("Created", "[Gender][eq][false]");
            var doubleQueryResult = await storyTable.MaxAsync<double>("Rating", "[Gender][eq][false]");
        }

        [Theory]
        [InlineData(0)]
        public async void Min(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolMinResult = await storyTable.MinAsync<bool>("Gender");
            var intMinResult = await storyTable.MinAsync<int>("Age");
            var stringMinResult = await storyTable.MinAsync<string>("Name");
            var datetimeMinResult = await storyTable.MinAsync<DateTime>("Created");
            var doubleMinResult = await storyTable.MinAsync<double>("Rating");
        }

        [Theory]
        [InlineData(0)]
        public async void MinQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolQueryMinResult = await storyTable.MinAsync<bool>("Gender", "[Gender][eq][true]");
            var intQueryMinResult = await storyTable.MinAsync<int>("Age", "[Gender][eq][true]");
            var stringQueryMinResult = await storyTable.MinAsync<string>("Name", "[Gender][eq][true]");
            var datetimeQueryMinResult = await storyTable.MinAsync<DateTime>("Created", "[Gender][eq][true]");
            var doubleQueryMinResult = await storyTable.MinAsync<double>("Rating", "[Gender][eq][false]");

        }

        [Theory]
        [InlineData(0)]
        public async void Sum(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolSumResult = await storyTable.SumAsync<bool>("Gender");
            var intSumResult = await storyTable.SumAsync<long>("Age");
            var doubleSumResult = await storyTable.SumAsync<double>("Rating");
        }

        [Theory]
        [InlineData(0)]
        public async void SumQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var boolQuerySumResult = await storyTable.SumAsync<bool>("Gender", "[Gender][eq][true]");
            var intQuerySumResult = await storyTable.SumAsync<int>("Age", "[Gender][eq][true]");
            var doubleQuerySumResult = await storyTable.SumAsync<double>("Rating", "[Gender][eq][false]");
        }

        [Theory]
        [InlineData(0)]
        public async void Avg(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var intAvgResult = await storyTable.AvgAsync("Age");
            var doubleAvgResult = await storyTable.AvgAsync("Rating");
        }

        [Theory]
        [InlineData(0)]
        public async void AvgQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            var intQueryAvgResult = await storyTable.AvgAsync("Age", "[Gender][eq][true]");
            var doubleQueryAvgResult = await storyTable.AvgAsync("Rating", "[Gender][eq][false]");
        }

        [Theory]
        [InlineData(0)]
        public async void First(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.FirstAsync("[Age][eq][33]", "age", 1);
            Profile profile1 = await storyTable.FirstAsync(sortfield: "age", sort: 1);
            Profile profile2 = await storyTable.FirstAsync(sortfield: "age");
            Profile profile3 = await storyTable.FirstAsync();
        }

        [Theory]
        [InlineData(0)]
        public async void Last(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile4 = await storyTable.LastAsync("[Age][eq][22]", "age", 1);
            Profile profile5 = await storyTable.LastAsync(sortfield: "age", sort: 1);
            Profile profile6 = await storyTable.LastAsync(sortfield: "age");
            Profile profile7 = await storyTable.LastAsync();
        }
    }
}
