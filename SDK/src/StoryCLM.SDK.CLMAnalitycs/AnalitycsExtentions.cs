using StoryCLM.SDK;
using StoryCLM.SDK.Extensions;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SroryCLM.SDK.CLMAnalitycs
{
    public static class AnalitycsExtentions
    {
        public static string Version = "v1";
        public static string Path = @"analitycs/clm";

        const string kAnalitycsSessions = @"sessions";
        const string kAnalitycsCustomEvents = @"customevents";
        const string kAnalitycsSlidesdemonstrations = @"slidesdemonstrations";

        const string kAnalitycsSessionsFeed = @"sessions/feed";
        const string kAnalitycsCustomEventsFeed = @"customevents/feed";
        const string kAnalitycsSlidesdemonstrationsFeed = @"slidesdemonstrations/feed";

        const string kAnalitycsSessionsCount = @"sessions/count";
        const string kAnalitycsCustomEventsCount = @"customevents/count";
        const string kAnalitycsSlidesdemonstrationsCount = @"slidesdemonstrations/count";

       static Uri GetUri(SCLM sclm, string query) =>
            new Uri($"{sclm.GetEndpoint("api")}/{Version}/{Path}/{query}", UriKind.Absolute);

        #region Sessions

        public static async Task<IEnumerable<StoryPresentationSession>> GetSessionsAsync(this SCLM sclm, 
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSessions}?");
            query.Append($"skip={skip}&take={take}&");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return await sclm.GETAsync<IEnumerable<StoryPresentationSession>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<IEnumerable<StoryPresentationSession>> GetSessionsFeedAsync(this SCLM sclm,
            DateTime? data = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSessionsFeed}?");
            query.Append($"skip={skip}&take={take}&");
            query.Append(data.ToQuery(nameof(data), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return await sclm.GETAsync<IEnumerable<StoryPresentationSession>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<long> GetSessionsCountAsync(this SCLM sclm, 
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSessionsCount}?");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return (await sclm.GETAsync<StoryCount>(GetUri(sclm, query.ToString()), CancellationToken.None)).Count;
        }

        #endregion

        #region Sessions

        public static async Task<IEnumerable<StorySlideDemonstration>> GetSlidesDemonstrationsAsync(this SCLM sclm,
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSlidesdemonstrations}?");
            query.Append($"skip={skip}&take={take}&");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return await sclm.GETAsync<IEnumerable<StorySlideDemonstration>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<IEnumerable<StorySlideDemonstration>> GetSlidesDemonstrationsFeedAsync(this SCLM sclm,
            DateTime? data = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSlidesdemonstrationsFeed}?");
            query.Append($"skip={skip}&take={take}&");
            query.Append(data.ToQuery(nameof(data), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return await sclm.GETAsync<IEnumerable<StorySlideDemonstration>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<long> GetSlidesDemonstrationsCountAsync(this SCLM sclm,
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            bool completeOnly = false)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsSlidesdemonstrationsCount}?");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"completeOnly={completeOnly}");
            return (await sclm.GETAsync<StoryCount>(GetUri(sclm, query.ToString()), CancellationToken.None)).Count;
        }

        #endregion

        #region CustomEvents

        public static async Task<IEnumerable<StoryCustomEvent>> GetCustomEventsAsync(this SCLM sclm, 
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsCustomEvents}?");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"skip={skip}&take={take}");
            return await sclm.GETAsync<IEnumerable<StoryCustomEvent>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<IEnumerable<StoryCustomEvent>> GetCustomEventsFeedAsync(this SCLM sclm,
            DateTime? data = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null,
            int skip = 0,
            int take = 100)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsCustomEventsFeed}?");
            query.Append(data.ToQuery(nameof(data), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            query.Append($"skip={skip}&take={take}");
            return await sclm.GETAsync<IEnumerable<StoryCustomEvent>>(GetUri(sclm, query.ToString()), CancellationToken.None);
        }

        public static async Task<long> GetCustomEventsCountAsync(this SCLM sclm, 
            DateTime? start = null,
            DateTime? finish = null,
            IEnumerable<int> presentationsIds = null,
            IEnumerable<string> usersIds = null)
        {
            StringBuilder query = new StringBuilder($"{kAnalitycsCustomEventsCount}?");
            query.Append(start.ToQuery(nameof(start), false));
            query.Append(finish.ToQuery(nameof(finish), false));
            query.Append(presentationsIds.ToQueryArray("pIds", false));
            query.Append(usersIds.ToQueryArray("uIds", false));
            return (await sclm.GETAsync<StoryCount>(GetUri(sclm, query.ToString()), CancellationToken.None)).Count;
        }

        #endregion
    }
}
