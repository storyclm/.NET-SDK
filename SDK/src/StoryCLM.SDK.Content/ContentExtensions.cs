using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public static class ContentExtensions
    {
        public static string Version = "v1";

        public static string PathClients = @"clients";
        public static string PathPresentations = @"presentations";
        public static string PathMediafiles = @"mediafiles";
        public static string PathSlides = @"slides";
        public static string PathContentpackages = @"contentpackages";

        const string api = nameof(api);

        public static async Task<IEnumerable<StoryClient>> GetClientsAsync(this SCLM sclm)
        {
            IEnumerable<StoryClient> result = await sclm.GETAsync<IEnumerable<StoryClient>>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathClients}", UriKind.Absolute), CancellationToken.None);
            foreach (var t in result) t._sclm = sclm;
            return result;
        }

        public static async Task<StoryClient> GetClientAsync(this SCLM sclm, int clientId)
        {
            StoryClient client = await sclm.GETAsync<StoryClient>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathClients}/{clientId}", UriKind.Absolute), CancellationToken.None);
            client._sclm = sclm;
            return client;
        }

        public static async Task<StoryPresentation> GetPresentationAsync(this SCLM sclm, int presentationId)
        {
            StoryPresentation presentation = await sclm.GETAsync<StoryPresentation>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathPresentations}/{presentationId}", UriKind.Absolute), CancellationToken.None);
            presentation._sclm = sclm;
            return presentation;
        }

        public static async Task<StoryMediafile> GetMediafileAsync(this SCLM sclm, int mediafileId) =>
            await sclm.GETAsync<StoryMediafile>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathMediafiles}/{mediafileId}", UriKind.Absolute), CancellationToken.None);

        public static async Task<StorySlide> GetSlideAsync(this SCLM sclm, int slideId) =>
            await sclm.GETAsync<StorySlide>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathSlides}/{slideId}", UriKind.Absolute), CancellationToken.None);

        public static async Task<StoryPackageSas> GetContentPackageAsync(this SCLM sclm, int presentationId) =>
            await sclm.GETAsync<StoryPackageSas>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{PathContentpackages}/{presentationId}", UriKind.Absolute), CancellationToken.None);
    }
}
