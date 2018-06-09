using StoryCLM.SDK;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StoryCLM.SDK.Extensions;

namespace Tests
{
    public class Analitycs
    {
        [Fact]
        public async void BaseTest()
        {
            SCLM sclm = await Utilities.GetContextAsync();

            IEnumerable<StoryPresentationSession> presentationsSessionsResult = await sclm.GetSessionsAsync();
            Assert.True(presentationsSessionsResult.Count() > 0);

            long presentationsSessionsResultCount = await sclm.GetSessionsCountAsync();
            Assert.True(presentationsSessionsResultCount > 0);

            IEnumerable<StoryCustomEvent> customEventResult = await sclm.GetCustomEventsAsync();
            Assert.True(customEventResult.Count() > 0);

            long customEventResultCount = await sclm.GetCustomEventsCountAsync();
            Assert.True(customEventResultCount > 0);
        }
    }
}
