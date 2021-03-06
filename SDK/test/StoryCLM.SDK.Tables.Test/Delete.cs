﻿using Shared;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Delete
    {
        [Theory]
        [InlineData(0)]
        public async void DeleteOne(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));
            IEnumerable<Profile> results = await storyTable.FindAsync(skip: 0, take: 1000);
            Assert.True(results.Count() > 0);

            Profile deleteResult = await storyTable.DeleteAsync(results.First()._id);
            Assert.NotNull(deleteResult);


        }

        [Theory]
        [InlineData(0)]
        public async void DeleteMany(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));
            var results = await storyTable.FindAsync(skip: 0, take: 1000);
            Assert.True(results.Count() > 0);

            IEnumerable<Profile> deleteResults = await storyTable.DeleteAsync(results.Select(t => t._id));
            Assert.True(deleteResults.Count() == results.Count());
        }

        [Theory]
        [InlineData(0)]
        public async void Clear(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);
            await storyTable.ClearAsync();
            Assert.True(await storyTable.CountAsync() == 0);
        }
    }
}
