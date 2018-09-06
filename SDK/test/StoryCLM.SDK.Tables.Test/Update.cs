using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Update
    {
        [Theory]
        [InlineData(0)]
        public async void UpdateOne(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());
            Assert.NotNull(profile);
            Profile updatedProfile = await storyTable.UpdateAsync(Profile.UpdateProfile(profile));

            Assert.NotNull(updatedProfile);
            Assert.False(profile.Equals(updatedProfile));
        }

        [Theory]
        [InlineData(0)]
        public async void UpdateMany(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 308);

            List<Profile> updatedProfiles = new List<Profile>(await storyTable.UpdateAsync(Profile.UpdateProfiles(profiles)));
            Assert.NotNull(updatedProfiles);
            Assert.True(updatedProfiles.Count() == 308);
        }
    }
}
