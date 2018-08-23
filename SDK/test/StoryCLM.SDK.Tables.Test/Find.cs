using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Find
    {
        [Theory]
        [InlineData(0)]
        public async void FindById(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            profile = await storyTable.FindAsync(profile._id);
            Assert.NotNull(profile);
        }

        [Theory]
        [InlineData(0)]
        public async void FindByIdRetryPolice(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            try
            {
                var profile = await storyTable.FindAsync("ADB902E5C84643AAA6EC7EC4A20934E4");
                Assert.True(false);
            }
            catch (Exception ex)
            {

            }
        }

        [Theory]
        [InlineData(0)]
        public async void FindByIds(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //Получить коллекцию записей по списку идентификаторов
            IEnumerable<Profile> results = await storyTable.FindAsync(profiles.Select(t => t._id).ToArray());
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            var results = await storyTable.FindAsync("[age][lte][30]", "age", 1, 0, 100);
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindSortFieldSortSkipTake(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            var results = await storyTable.FindAsync(sortfield: "age", sort: 1, skip: 0, take: 100);
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindSortSkipTake(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            var results = await storyTable.FindAsync(sortfield: "age", skip: 0, take: 100);
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindSkipTake(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            var results = await storyTable.FindAsync(skip: 0, take: 100);
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindAll(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            var results = await storyTable.FindAsync();
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindQueryLTE(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //возраст меньше или равен 30
            var results = storyTable.FindAsync("[age][lte][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindQuerySW(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //поле "name" начинается с символа "T"
            var results = storyTable.FindAsync("[name][sw][\"T\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindQueryCN(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //поле "name" содержит строку "ad"
            var results = storyTable.FindAsync("[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindQueryIN(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //поиск имен из списка
            var results = storyTable.FindAsync("[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindEQANDSW(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //Выбрать женщин, имя ("name") которых начинается со строки "V" 
            var results = storyTable.FindAsync("[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindLongQuery(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //Выбрать мужчин младше 30 и женщин старше 30
            var results = storyTable.FindAsync("[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(0)]
        public async void FindBrackets(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var storyTable = await Tables.GetTableAsync(sclm);

            Profile profile = await storyTable.InsertAsync(Profile.CreateProfile());

            Profile profile1 = await storyTable.InsertAsync(Profile.CreateProfile1());
            Profile profile2 = await storyTable.InsertAsync(Profile.CreateProfile2());
            Profile profile3 = await storyTable.InsertAsync(Profile.CreateProfile3());
            Profile profile4 = await storyTable.InsertAsync(Profile.CreateProfile4());
            List<Profile> profiles = new List<Profile>(await storyTable.InsertAsync(Profile.CreateProfiles()));

            //поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
            var results = storyTable.FindAsync("([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
            results = storyTable.FindAsync("([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
            // Assert.True(results.Count() > 0);
        }

    }
}
