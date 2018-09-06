using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Insert
    {
        [Theory]
        [InlineData(0)]
        public async void InsertOne(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            await storyTable.InsertAsync(new Profile());
            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());
            Assert.NotNull(profile);
        }

        [Theory]
        [InlineData(0)]
        public async void InsertMany(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 308);
        }
    }
}
