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
        const int tableId = 23;

        [Theory]
        [InlineData(18)]
        public async void GetTables(int id)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(id);
            ApiTable table = tables.FirstOrDefault(t => t.Name.Contains("Profile"));
            Assert.True(tables.Count() > 0);
            Assert.NotNull(table);
        }

        [Theory]
        [InlineData(tableId)]
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
        [InlineData(tableId)]
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
        [InlineData(tableId)]
        public async void Count(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            long count = await sclm.CountAsync(tableId);
            Assert.True(count > 0);
            count = await sclm.CountAsync(tableId, "[Gender][eq][false]");
            Assert.True(count > 0);
            count = await sclm.LogCountAsync(tableId);
            Assert.True(count > 0);
            count = await sclm.LogCountAsync(tableId, DateTime.Now.AddDays(-5));
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(tableId)]
        public async void Log(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);

            IEnumerable<ApiLog> logs = await sclm.LogAsync(tableId, DateTime.Now.AddDays(-5), 0, 900);
            Assert.True(logs.Count() > 0);

            logs = await sclm.LogAsync(tableId, DateTime.Now.AddDays(-5));
            Assert.True(logs.Count() > 0);

            logs = await sclm.LogAsync(tableId, skip: 0, take: 900);
            Assert.True(logs.Count() > 0);

            logs = await sclm.LogAsync(tableId);
            Assert.True(logs.Count() > 0);
        }

        [Theory]
        [InlineData(tableId)]
        public async void Aggregation(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);

            var boolResult = await sclm.MaxAsync<bool>(tableId, "Gender");
            var intResult = await sclm.MaxAsync<int>(tableId, "Age");
            var stringResult = await sclm.MaxAsync<string>(tableId, "Name");
            var datetimeResult = await sclm.MaxAsync<DateTime>(tableId, "Created");
            var doubleResult = await sclm.MaxAsync<double>(tableId, "Rating");

            var boolQueryResult = await sclm.MaxAsync<bool>(tableId, "Gender", "[Gender][eq][false]");
            var intQueryResult = await sclm.MaxAsync<int>(tableId, "Age", "[Gender][eq][false]");
            var stringQueryResult = await sclm.MaxAsync<string>(tableId, "Name", "[Gender][eq][false]");
            var datetimeQueryResult = await sclm.MaxAsync<DateTime>(tableId, "Created", "[Gender][eq][false]");
            var doubleQueryResult = await sclm.MaxAsync<double>(tableId, "Rating", "[Gender][eq][false]");

            var boolMinResult = await sclm.MinAsync<bool>(tableId, "Gender");
            var intMinResult = await sclm.MinAsync<int>(tableId, "Age");
            var stringMinResult = await sclm.MinAsync<string>(tableId, "Name");
            var datetimeMinResult = await sclm.MinAsync<DateTime>(tableId, "Created");
            var doubleMinResult = await sclm.MinAsync<double>(tableId, "Rating");

            var boolQueryMinResult = await sclm.MinAsync<bool>(tableId, "Gender", "[Gender][eq][true]");
            var intQueryMinResult = await sclm.MinAsync<int>(tableId, "Age", "[Gender][eq][true]");
            var stringQueryMinResult = await sclm.MinAsync<string>(tableId, "Name", "[Gender][eq][true]");
            var datetimeQueryMinResult = await sclm.MinAsync<DateTime>(tableId, "Created", "[Gender][eq][true]");
            var doubleQueryMinResult = await sclm.MinAsync<double>(tableId, "Rating", "[Gender][eq][false]");

            var boolSumResult = await sclm.SumAsync<bool>(tableId, "Gender");
            var intSumResult = await sclm.SumAsync<long>(tableId, "Age");
            var doubleSumResult = await sclm.SumAsync<double>(tableId, "Rating");

            var boolQuerySumResult = await sclm.SumAsync<bool>(tableId, "Gender", "[Gender][eq][true]");
            var intQuerySumResult = await sclm.SumAsync<int>(tableId, "Age", "[Gender][eq][true]");
            var doubleQuerySumResult = await sclm.SumAsync<double>(tableId, "Rating", "[Gender][eq][false]");

            var intAvgResult = await sclm.AvgAsync(tableId, "Age");
            var doubleAvgResult = await sclm.AvgAsync(tableId, "Rating");

            var intQueryAvgResult = await sclm.AvgAsync(tableId, "Age", "[Gender][eq][true]");
            var doubleQueryAvgResult = await sclm.AvgAsync(tableId, "Rating", "[Gender][eq][false]");

            Profile profile = await sclm.FirstAsync<Profile>(tableId, "[Age][eq][33]", "age", 1);
            Profile profile1 = await sclm.FirstAsync<Profile>(tableId, sortfield: "age", sort: 1);
            Profile profile2 = await sclm.FirstAsync<Profile>(tableId, sortfield: "age");
            Profile profile3 = await sclm.FirstAsync<Profile>(tableId);

            Profile profile4 = await sclm.LastAsync<Profile>(tableId, "[Age][eq][22]", "age", 1);
            Profile profile5 = await sclm.LastAsync<Profile>(tableId, sortfield: "age", sort: 1);
            Profile profile6 = await sclm.LastAsync<Profile>(tableId, sortfield: "age");
            Profile profile7 = await sclm.LastAsync<Profile>(tableId);
        }

        [Theory]
        [InlineData(tableId)]
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

            //Получить объект по идентификатору
            profile = await sclm.FindAsync<Profile>(tableId, profile._id);
            Assert.NotNull(profile);

            //Получить коллекцию записей по списку идентификаторов
            IEnumerable<Profile>  results = await sclm.FindAsync<Profile>(tableId, profiles.Select(t => t._id).ToArray());
            Assert.True(results.Count() > 0);

            results = await sclm.FindAsync<Profile>(tableId, "[Gender][eq][false]", "age", 1, 0, 100);
            Assert.True(results.Count() > 0);

            results = await sclm.FindAsync<Profile>(tableId, sortfield: "age", sort: 1, skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await sclm.FindAsync<Profile>(tableId, sortfield: "age", skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await sclm.FindAsync<Profile>(tableId, skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await sclm.FindAsync<Profile>(tableId);
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

        }

        [Theory]
        [InlineData(tableId)]
        public async Task Delete(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<Profile> results = await sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000);
            Assert.True(results.Count() > 0);

            Profile deleteResult = await sclm.DeleteAsync<Profile>(tableId, results.First()._id);
            Assert.NotNull(deleteResult);

            results = await sclm.FindAsync<Profile>(tableId, skip: 0, take: 10);
            Assert.True(results.Count() > 0);

            IEnumerable<Profile> deleteResults = await sclm.DeleteAsync<Profile>(tableId, results.Select(t=> t._id));
            Assert.True(deleteResults.Count() > 0);
        }

        [Theory]
        [InlineData(tableId)]
        public async void Full(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, skip: 0, take: 1000).Result;
            Assert.True(results.Count() > 0);
        }

        
    }

    //nuget pack D:\.NET-SDK\SDK\StoryCLM.SDK\StoryCLM.SDK.csproj -properties Configuration=Release

}
