using StoryCLM.SDK.CLMAnalitycs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public static class AnalitycsExtentions
    {
        internal const string Version = "v1";
        internal const string Path = @"analitycs/clm";

        const string kAnalitycsSessions = @"sessions";
        const string kAnalitycsCustomEvents = @"customevents";
        const string kAnalitycsSlidesdemonstrations = @"slides";

        const string kAnalitycskpis = @"kpis";
        const string kAnalitycskpip = @"kpip";


        static Uri GetUri(SCLM sclm, string query) =>
            new Uri($"{sclm.GetEndpoint("api")}{Version}/{Path}/{query}", UriKind.Absolute);


        public static CLMAnalitycsFeed<StorySessionEvent> GetSessionFeed(this SCLM sclm, 
            int presentationId,
            string userId = null,
            Section section = null,
            long? continuationToken = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CLMAnalitycsFeed<StorySessionEvent>(kAnalitycsSessions, presentationId, userId, section, continuationToken)
            {
                _sclm = sclm,
                CancellationToken = cancellationToken
            };
        }

        public static CLMAnalitycsFeed<StorySlideEvent> GetSlidesFeed(this SCLM sclm,
            int presentationId,
            string userId = null,
            Section section = null,
            long? continuationToken = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CLMAnalitycsFeed<StorySlideEvent>(kAnalitycsSlidesdemonstrations, presentationId, userId, section, continuationToken)
            {
                _sclm = sclm,
                CancellationToken = cancellationToken
            };
        }

        public static CLMAnalitycsFeed<StoryCustomEvent> GetCustomEventsFeed(this SCLM sclm,
            int presentationId,
            string userId = null,
            Section section = null,
            long? continuationToken = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CLMAnalitycsFeed<StoryCustomEvent>(kAnalitycsCustomEvents, presentationId, userId, section, continuationToken)
            {
                _sclm = sclm,
                CancellationToken = cancellationToken
            };
        }


        #region KPI

        internal static Uri GetUri(Uri endpoint, int presentationId, string userId = null)
        {
            var builder = new UriBuilder(endpoint);
            NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["presentationId"] = presentationId.ToString();

            if (!string.IsNullOrEmpty(userId))
                parameters["userId"] = userId;

            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        public static async Task<StoryKPISEvent> GetKPIS(this SCLM sclm,
            int presentationId,
            string userId = null,
            Section section = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Uri uri = GetUri(new Uri($"{sclm.GetEndpoint("api")}{Version}/{Path}/{kAnalitycskpis}/{section}", UriKind.Absolute), presentationId, userId);
            return await sclm.GETAsync<StoryKPISEvent>(uri, CancellationToken.None);
        }

        public static async Task<StoryKPIPEvent> GetKPIP(this SCLM sclm,
            int presentationId,
            string userId = null,
            Section section = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Uri uri = GetUri(new Uri($"{sclm.GetEndpoint("api")}{Version}/{Path}/{kAnalitycskpip}/{section}", UriKind.Absolute), presentationId, userId);
            return await sclm.GETAsync<StoryKPIPEvent>(uri, CancellationToken.None);
        }

        public static async Task SetKPIS(this SCLM sclm, StoryKPISEvent @event, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.PUTAsync<StoryCustomEvent>(GetUri(sclm, kAnalitycskpis), @event, cancellationToken);

        public static async Task SetKPIP(this SCLM sclm, StoryKPIPEvent @event, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.PUTAsync<StoryCustomEvent>(GetUri(sclm, kAnalitycskpip), @event, cancellationToken);

        #endregion

        public static async Task SendSessionEvent(this SCLM sclm, StorySessionEvent @event, CancellationToken cancellationToken = default(CancellationToken)) => 
            await sclm.PUTAsync<StorySessionEvent>(GetUri(sclm, kAnalitycsSessions), @event, cancellationToken);

        public static async Task SendSlideEvent(this SCLM sclm, StorySlideEvent @event, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.PUTAsync<StorySlideEvent>(GetUri(sclm, kAnalitycsSlidesdemonstrations), @event, cancellationToken);

        public static async Task SendCustomEvent(this SCLM sclm, StoryCustomEvent @event, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.PUTAsync<StoryCustomEvent>(GetUri(sclm, kAnalitycsCustomEvents), @event, cancellationToken);



        public static async Task<StoryVisit<T>> GetVisit<T>(this SCLM sclm, string sessionId, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.GETAsync<StoryVisit<T>>(GetUri(sclm, $"visits/{sessionId}"), cancellationToken);

        public static async Task<T> GetForm<T>(this SCLM sclm, string sessionId, CancellationToken cancellationToken = default(CancellationToken)) where T : class =>
            await sclm.GETAsync<T>(GetUri(sclm, $"form/{sessionId}"), cancellationToken);



        public static async Task<StorySessionEvent> GetSessionEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.GETAsync<StorySessionEvent>(GetUri(sclm, $"session/{id}"), cancellationToken);

        public static async Task<StorySlideEvent> GetSlideEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.GETAsync<StorySlideEvent>(GetUri(sclm, $"slide/{id}"), cancellationToken);

        public static async Task<StoryCustomEvent> GetCustomEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.GETAsync<StoryCustomEvent>(GetUri(sclm, $"customevent/{id}"), cancellationToken);



        public static async Task DeleteSessionEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.DELETEAsync<StorySessionEvent>(GetUri(sclm, $"sessions/{id}"), cancellationToken);

        public static async Task DeleteSlideEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.DELETEAsync<StorySlideEvent>(GetUri(sclm, $"slides/{id}"), cancellationToken);

        public static async Task DeleteCustomEvent(this SCLM sclm, string id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await sclm.DELETEAsync<StoryCustomEvent>(GetUri(sclm, $"customevents/{id}"), cancellationToken);

    }
}
