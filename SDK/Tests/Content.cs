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
using Xunit;

namespace Tests
{
    public class Content
    {
        [Theory]
        [InlineData(18)]
        public async void GetContent(int id)
        {
            SCLM sclm = await Utilities.GetContextAsync();

            IEnumerable<StoryClient> clients = await sclm.GetClientsAsync();
            Assert.True(clients.Count() > 0);

            foreach (StoryClient c in clients)
            {
                StoryClient client = await sclm.GetClientAsync(c.Id);
                Assert.NotNull(client);
                foreach (var p in client.Presentations)
                {
                    StoryPresentation presentation = await client.GetPresentationAsync(p.Id);
                    Assert.NotNull(presentation);
                    foreach (var m in presentation.MediaFiles)
                    {
                        StoryMediafile mediafile = await presentation.GetMediafileAsync(m.Id);
                        Assert.NotNull(mediafile);
                    }
                    foreach (var s in presentation.Slides)
                    {
                        StorySlide slide = await presentation.GetSlideAsync(s.Id);
                        Assert.NotNull(slide);
                    }
                    StoryPackageSas psas = await presentation.GetContentPackageAsync();
                }
            }
        }

        const string adminUserId = "f150469c-09d1-4d5b-b99f-9abc39a69414";
        const string managerUserId = "3ab4a1db-b434-4b00-bbd8-253863042bac";
        const string userFromAnotheClientId = "";

        [Theory]
        [InlineData(4991)]
        public async void UsersForPresentationsAsync(int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync();

            string[] usersIds = { "31e806b7-56b2-4560-ad39-2fa1a382a9d2", "d0532be9-d6d8-4401-8155-01e309f87aa7", "b2b68ca3-4e7b-4e36-b29e-dbcc06585065" };

            //получаем презентацию
            StoryPresentation presentation = await sclm.GetPresentationAsync(presentationId);
            Assert.NotNull(presentation);

            await presentation.RemoveAllUsersAsync();
            Assert.True(presentation.Users.Count() == 0);

            var users = await presentation.AddUsersAsync(usersIds);
            Assert.True(presentation.Users.Count() == 3);

            users = await presentation.AddUsersAsync(usersIds);
            Assert.True(presentation.Users.Count() == 3);


        }

    }
}
