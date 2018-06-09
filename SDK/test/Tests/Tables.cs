/*!
* StoryCLM.SDK Library v1.6.0
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

 

        [Theory]
        [InlineData(18)]
        public async void Find(int e)
        {
            SCLM sclm = await Utilities.GetContextAsync();
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
            SCLM sclm = await Utilities.GetContextAsync();
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
