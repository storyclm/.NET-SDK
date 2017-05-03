/*!
* StoryCLM.SDK Library v1.0.0
* Copyright(c) 2017, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Models;
using Xunit;

namespace Tests
{
   public class Tables
    {
        const string clientId = "client_18";
        const string secret = "595a2fb724604e51a1f9e43b808c76c915c2e0f74e8840b384218a0e354f6de6";

        [Fact]
        public async void GetTables()
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(18);
            ApiTable table = tables.FirstOrDefault(t => t.Name.Contains("Profile"));
            Assert.True(tables.Count() > 0);
            Assert.NotNull(table);
        }

        [Theory]
        [InlineData(23)]
        public async void Insert(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            Profile profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile());
            Assert.NotNull(profile);
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfiles()));
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);
        }

        [Theory]
        [InlineData(23)]
        public async void Update(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            Profile profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile());
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfiles()));

            Assert.NotNull(profile);
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);

            Profile updatedProfile = await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfile(profile));
            List<Profile> updatedProfiles = new List<Profile>(await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfiles(profiles)));

            Assert.NotNull(updatedProfile);
            Assert.NotNull(updatedProfiles);
            Assert.True(updatedProfiles.Count() == 3);
            Assert.False(profile.Equals(updatedProfile));
        }

        [Theory]
        [InlineData(23)]
        public async void Count(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            long count = await sclm.CountAsync(tableId);
            Assert.True(count > 0);
            count = await sclm.LogCountAsync(tableId, (DateTime.Now.AddDays(-25)));
            Assert.True(count > 0);
            count = await sclm.CountAsync(tableId, "[age][gt][30]");
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(23)]
        public async void Find(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            Profile profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile());
            profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile1());
            profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile2());
            profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile3());
            profile = await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfiles()));

            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            //возраст меньше или равен 30
            results = sclm.FindAsync<Profile>(tableId, "[age][lte][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" начинается с символа "T"
            results = sclm.FindAsync<Profile>(tableId, "[name][sw][\"T\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" содержит строку "ad"
            results = sclm.FindAsync<Profile>(tableId, "[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поиск имен из списка
            results = sclm.FindAsync<Profile>(tableId, "[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать женщин, имя ("name") которых начинается со строки "V" 
            results = sclm.FindAsync<Profile>(tableId, "[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать мужчин младше 30 и женщин старше 30
            results = sclm.FindAsync<Profile>(tableId, "[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            #region сложные запросы со скобочками

            //поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
            results = sclm.FindAsync<Profile>(tableId, "([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
            results = sclm.FindAsync<Profile>(tableId, "([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
           // Assert.True(results.Count() > 0);

            #endregion

            //Получить запись по идентификатору
            profile = await sclm.FindAsync<Profile>(tableId, profile._id);
            Assert.NotNull(profile);

            ////Получить коллекцию записей по списку идентификаторов
            results = await sclm.FindAsync<Profile>(tableId, profiles.Select(t => t._id).ToArray());
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(23)]
        public async Task Delete(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<Profile> results = await sclm.FindAsync<Profile>(tableId, 0, 1000);
            Assert.True(results.Count() > 0);

            Profile deleteResult = await sclm.DeleteAsync<Profile>(tableId, results.First()._id);
            Assert.NotNull(deleteResult);

            results = await sclm.FindAsync<Profile>(tableId, 0, 100);
            Assert.True(results.Count() > 0);

            IEnumerable<Profile> deleteResults = await sclm.DeleteAsync<Profile>(tableId, results.Select(t=> t._id));
            Assert.True(deleteResults.Count() > 0);
        }

        [Theory]
        [InlineData(23)]
        public async void Full(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);
        }

        
    }

    //nuget pack D:\.NET-SDK\SDK\StoryCLM.SDK\StoryCLM.SDK.csproj -properties Configuration=Release

}
