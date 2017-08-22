/*!
* StoryCLM.SDK Library v1.6.0
* Copyright(c) 2017, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
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
    }
}
