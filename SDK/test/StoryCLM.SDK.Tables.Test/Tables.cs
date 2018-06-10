using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Tables.Test
{
    public class Tables
    {
        const int _clientId = 18;
        const int _tableId = 23;
        const string _tableName = "Profile";

        public static async Task<StoryTable<Profile>> GetTableAsync(SCLM sclm) =>
            await sclm.GetTableAsync<Profile>(_tableId);
    

        [Theory]
        [InlineData(_clientId, 0, _tableName)]
        public async void GetAllClientTables(int clientId, int uc, string tableName)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            IEnumerable<StoryTable<Profile>> tables = await sclm.GetTablesAsync<Profile>(clientId);
            Assert.True(tables.Count() > 0);

            StoryTable<Profile> table = tables.FirstOrDefault(t => t.Name.Contains(tableName));
            Assert.NotNull(table);
        }

        [Theory]
        [InlineData(_tableId, 0)]
        public async void GetTableById(int tableId, int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
            Assert.NotNull(table);
            Assert.Equal(_tableName, table.Name);
            Assert.Equal(_tableId, table.Id);
        }

        //[Theory]
        //[InlineData(_tableName, 0)]
        //public async void GetTableByName(string tableName, int uc)
        //{
        //    SCLM sclm = await Utilities.GetContextAsync(uc);

        //    StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableName);
        //    Assert.NotNull(table);
        //    Assert.Equal(_tableName, table.Name);
        //    Assert.Equal(_tableId, table.Id);
        //}

        [Theory]
        [InlineData(_tableId, 0)]
        public async void Schema(int tableId, int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
            Assert.NotNull(table);
            Assert.NotNull(table.Schema);
            Assert.Equal(5, table.Schema.Count());
        }
    }
}
