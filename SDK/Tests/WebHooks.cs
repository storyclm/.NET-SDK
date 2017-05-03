/*!
* StoryCLM.SDK Library v0.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using StoryCLM.SDK;
using Tests.Models;
using Xunit;

namespace Tests
{
   public class WebHooks
    {
        [Theory]
        [InlineData("92126aaef4ad4a899146faf8d3bfa0fa", "5908400792199a03dc56415e", "application/json")]
        public async void TablesJsonRecevierHandler(string id, string key, string contentType)
        {
            SCLM sclm = SCLM.Instance;
            Profile profile = Profile.CreateProfile();

            string insertProfileResult = await sclm.WebHookAsync(id, key, JsonConvert.SerializeObject(profile, Formatting.Indented), contentType);

            Profile updateProfile =  Profile.UpdateProfile(JsonConvert.DeserializeObject<Profile>(insertProfileResult));

            string updateProfileResult = await sclm.WebHookAsync(id, key, JsonConvert.SerializeObject(updateProfile, Formatting.Indented), contentType);

        }
    }

}
