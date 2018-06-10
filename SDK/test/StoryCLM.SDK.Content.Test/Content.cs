using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Content.Test
{
    public class Content
    {
        [Theory]
        [InlineData(0)]
        public async void GetClients(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var clients = await sclm.GetClientsAsync();
            Assert.True(clients.Count() > 0);
        }

        [Theory]
        [InlineData(0, 18)]
        public async void GetClient(int uc, int clientId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var client = await sclm.GetClientAsync(clientId);
            Assert.NotNull(client);
        }

        [Theory]
        [InlineData(0, 18)]
        public async void GetPresentationFromClient(int uc, int clientId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var client = await sclm.GetClientAsync(clientId);
            var pres = client.Presentations.First();
            Assert.NotNull(pres);
            //var presentation = await client.GetPresentationAsync(pres.Id);
            //Assert.NotNull(presentation);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetPresentaion(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            Assert.NotNull(presentation);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetGroup(int uc, int groupId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

        }










        //[Theory]
        //[InlineData(18)]
        //public async void GetContent(int id)
        //{
        //    SCLM sclm = await Utilities.GetContextAsync();

        //    IEnumerable<StoryClient> clients = await sclm.GetClientsAsync();
        //    Assert.True(clients.Count() > 0);

        //    foreach (StoryClient c in clients)
        //    {
        //        StoryClient client = await sclm.GetClientAsync(c.Id);
        //        Assert.NotNull(client);
        //        foreach (var p in client.Presentations)
        //        {
        //            StoryPresentation presentation = await client.GetPresentationAsync(p.Id);
        //            Assert.NotNull(presentation);
        //            foreach (var m in presentation.MediaFiles)
        //            {
        //                StoryMediafile mediafile = await presentation.GetMediafileAsync(m.Id);
        //                Assert.NotNull(mediafile);
        //            }
        //            foreach (var s in presentation.Slides)
        //            {
        //                StorySlide slide = await presentation.GetSlideAsync(s.Id);
        //                Assert.NotNull(slide);
        //            }
        //            StoryPackageSas psas = await presentation.GetContentPackageAsync();
        //        }
        //    }
        //}

        //const string adminUserId = "f150469c-09d1-4d5b-b99f-9abc39a69414";
        //const string managerUserId = "3ab4a1db-b434-4b00-bbd8-253863042bac";
        //const string userFromAnotheClientId = "";

        //[Theory]
        //[InlineData(4991)]
        //public async void UsersForPresentationsAsync(int presentationId)
        //{
        //    SCLM sclm = await Utilities.GetContextAsync();

        //    string[] usersIds = { "31e806b7-56b2-4560-ad39-2fa1a382a9d2", "d0532be9-d6d8-4401-8155-01e309f87aa7", "b2b68ca3-4e7b-4e36-b29e-dbcc06585065" };

        //    //получаем презентацию
        //    StoryPresentation presentation = await sclm.GetPresentationAsync(presentationId);
        //    Assert.NotNull(presentation);

        //    await presentation.RemoveAllUsersAsync();
        //    Assert.True(presentation.Users.Count() == 0);

        //    var users = await presentation.AddUsersAsync(usersIds);
        //    Assert.True(presentation.Users.Count() == 3);

        //    users = await presentation.AddUsersAsync(usersIds);
        //    Assert.True(presentation.Users.Count() == 3);


        //}



    }
}
