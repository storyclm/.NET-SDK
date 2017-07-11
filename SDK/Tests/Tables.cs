/*!
* StoryCLM.SDK Library v1.3.0
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
        const int uc = 0;
        const int clientId = 18;
        const int tableId = 23;


        async Task<SCLM> GetContextAsync()
        {
            switch (uc)
            {
                case 0: // service
                    {
                        string clientId = "client_18_1";
                        string secret = "";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(clientId, secret);
                        return sclm;
                    }
                case 1: // application (user)
                    {
                        string username = "rsk-k161@ya.ru";
                        string password = "";
                        string clientId = "client_18_4";
                        string secret = "";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(clientId, secret, username, password);
                        return sclm;
                    }
            }
            return null;
        }

        async Task<StoryTable<Profile>> GetTableAsync(SCLM sclm)
        {
            IEnumerable<StoryTable<Profile>> tables = await sclm.GetTablesAsync<Profile>(clientId);
            StoryTable<Profile> table = tables.FirstOrDefault(t => t.Name.Contains("Profile"));
            return table;
        }

        [Theory]
        [InlineData(18)]
        public async void GetTables(int id)
        {
            SCLM sclm = await GetContextAsync();
            IEnumerable<StoryTable<Profile>> tables = await sclm.GetTablesAsync<Profile>(id);
            StoryTable<Profile> table = tables.FirstOrDefault(t => t.Name.Contains("Profile"));
            Assert.True(tables.Count() > 0);
            Assert.NotNull(table);
        }

        [Theory]
        [InlineData(18)]
        public async void Insert(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            await storyTable.InsertAsync(new Profile());
            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());
            Assert.NotNull(profile);
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);
        }

        [Theory]
        [InlineData(18)]
        public async void Update(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            Assert.NotNull(profile);
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);

            Profile updatedProfile = await storyTable.UpdateAsync(Profile.UpdateProfile(profile));
            List<Profile> updatedProfiles = new List<Profile>(await storyTable.UpdateAsync(Profile.UpdateProfiles(profiles)));

            Assert.NotNull(updatedProfile);
            Assert.NotNull(updatedProfiles);
            Assert.True(updatedProfiles.Count() == 3);
            Assert.False(profile.Equals(updatedProfile));
        }

        [Theory]
        [InlineData(18)]
        public async void Count(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            long count = await storyTable.CountAsync();
            Assert.True(count > 0);
            count = await storyTable.CountAsync("[Gender][eq][false]");
            Assert.True(count > 0);
            count = await storyTable.LogCountAsync();
            Assert.True(count > 0);
            count = await storyTable.LogCountAsync(DateTime.Now.AddDays(-5));
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(18)]
        public async void Log(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            IEnumerable<ApiLog> logs = await storyTable.LogAsync(DateTime.Now.AddDays(-5), 0, 900);
            Assert.True(logs.Count() > 0);

            logs = await storyTable.LogAsync(DateTime.Now.AddDays(-5));
            Assert.True(logs.Count() > 0);

            logs = await storyTable.LogAsync(skip: 0, take: 900);
            Assert.True(logs.Count() > 0);

            logs = await storyTable.LogAsync();
            Assert.True(logs.Count() > 0);
        }

        [Theory]
        [InlineData(18)]
        public async void Aggregation(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);

            var boolResult = await storyTable.MaxAsync<bool>("Gender");
            var intResult = await storyTable.MaxAsync<int>("Age");
            var stringResult = await storyTable.MaxAsync<string>("Name");
            var datetimeResult = await storyTable.MaxAsync<DateTime>("Created");
            var doubleResult = await storyTable.MaxAsync<double>("Rating");

            var boolQueryResult = await storyTable.MaxAsync<bool>("Gender", "[Gender][eq][false]");
            var intQueryResult = await storyTable.MaxAsync<int>("Age", "[Gender][eq][false]");
            var stringQueryResult = await storyTable.MaxAsync<string>("Name", "[Gender][eq][false]");
            var datetimeQueryResult = await storyTable.MaxAsync<DateTime>("Created", "[Gender][eq][false]");
            var doubleQueryResult = await storyTable.MaxAsync<double>("Rating", "[Gender][eq][false]");

            var boolMinResult = await storyTable.MinAsync<bool>("Gender");
            var intMinResult = await storyTable.MinAsync<int>("Age");
            var stringMinResult = await storyTable.MinAsync<string>("Name");
            var datetimeMinResult = await storyTable.MinAsync<DateTime>("Created");
            var doubleMinResult = await storyTable.MinAsync<double>("Rating");

            var boolQueryMinResult = await storyTable.MinAsync<bool>("Gender", "[Gender][eq][true]");
            var intQueryMinResult = await storyTable.MinAsync<int>("Age", "[Gender][eq][true]");
            var stringQueryMinResult = await storyTable.MinAsync<string>("Name", "[Gender][eq][true]");
            var datetimeQueryMinResult = await storyTable.MinAsync<DateTime>("Created", "[Gender][eq][true]");
            var doubleQueryMinResult = await storyTable.MinAsync<double>("Rating", "[Gender][eq][false]");

            var boolSumResult = await storyTable.SumAsync<bool>("Gender");
            var intSumResult = await storyTable.SumAsync<long>("Age");
            var doubleSumResult = await storyTable.SumAsync<double>("Rating");

            var boolQuerySumResult = await storyTable.SumAsync<bool>("Gender", "[Gender][eq][true]");
            var intQuerySumResult = await storyTable.SumAsync<int>("Age", "[Gender][eq][true]");
            var doubleQuerySumResult = await storyTable.SumAsync<double>("Rating", "[Gender][eq][false]");

            var intAvgResult = await storyTable.AvgAsync("Age");
            var doubleAvgResult = await storyTable.AvgAsync("Rating");

            var intQueryAvgResult = await storyTable.AvgAsync("Age", "[Gender][eq][true]");
            var doubleQueryAvgResult = await storyTable.AvgAsync("Rating", "[Gender][eq][false]");

            Profile profile = await storyTable.FirstAsync("[Age][eq][33]", "age", 1);
            Profile profile1 = await storyTable.FirstAsync(sortfield: "age", sort: 1);
            Profile profile2 = await storyTable.FirstAsync(sortfield: "age");
            Profile profile3 = await storyTable.FirstAsync();

            Profile profile4 = await storyTable.LastAsync("[Age][eq][22]", "age", 1);
            Profile profile5 = await storyTable.LastAsync(sortfield: "age", sort: 1);
            Profile profile6 = await storyTable.LastAsync(sortfield: "age");
            Profile profile7 = await storyTable.LastAsync();
        }

        [Theory]
        [InlineData(18)]
        public async void Find(int e)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());
            profile = await storyTable.InsertAsync(Profile.CreateProfile1());
            profile = await storyTable.InsertAsync(Profile.CreateProfile2());
            profile = await storyTable.InsertAsync(Profile.CreateProfile3());
            profile = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //Получить объект по идентификатору
            profile = await storyTable.FindAsync(profile._id);
            Assert.NotNull(profile);

            //Получить коллекцию записей по списку идентификаторов
            IEnumerable<Profile>  results = await storyTable.FindAsync(profiles.Select(t => t._id).ToArray());
            Assert.True(results.Count() > 0);

            results = await storyTable.FindAsync("[age][lte][30]", "age", 1, 0, 100);
            Assert.True(results.Count() > 0);

            results = await storyTable.FindAsync(sortfield: "age", sort: 1, skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await storyTable.FindAsync(sortfield: "age", skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await storyTable.FindAsync(skip: 0, take: 100);
            Assert.True(results.Count() > 0);

            results = await storyTable.FindAsync();
            Assert.True(results.Count() > 0);

            //возраст меньше или равен 30
            results = storyTable.FindAsync("[age][lte][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" начинается с символа "T"
            results = storyTable.FindAsync("[name][sw][\"T\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" содержит строку "ad"
            results = storyTable.FindAsync("[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поиск имен из списка
            results = storyTable.FindAsync("[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать женщин, имя ("name") которых начинается со строки "V" 
            results = storyTable.FindAsync("[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать мужчин младше 30 и женщин старше 30
            results = storyTable.FindAsync("[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            #region сложные запросы со скобочками

            //поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
            results = storyTable.FindAsync("([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
            results = storyTable.FindAsync("([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
           // Assert.True(results.Count() > 0);

            #endregion

        }

        [Theory]
        [InlineData(18)]
        public async Task Delete(int id)
        {
            SCLM sclm = await GetContextAsync();
            var storyTable = await GetTableAsync(sclm);
            IEnumerable<Profile> results = await storyTable.FindAsync(skip: 0, take: 1000);
            Assert.True(results.Count() > 0);

            Profile deleteResult = await storyTable.DeleteAsync(results.First()._id);
            Assert.NotNull(deleteResult);

            results = await storyTable.FindAsync(skip: 0, take: 60);
            Assert.True(results.Count() > 0);

            IEnumerable<Profile> deleteResults = await storyTable.DeleteAsync(results.Select(t=> t._id));
            Assert.True(deleteResults.Count() > 0);
        }

    }

}
