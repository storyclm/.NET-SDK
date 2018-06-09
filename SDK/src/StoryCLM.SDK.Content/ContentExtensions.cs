using System;
using System.Collections.Generic;
using System.Text;
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

        public static async Task<IEnumerable<StoryClient>> GetClientsAsync(this SCLM sclm)
        {
            IEnumerable<StoryClient> result = await sclm.GETAsync<IEnumerable<StoryClient>>(new Uri($"{sclm.Endpoint}/{Version}/{PathClients}", UriKind.Absolute));
            foreach (var t in result) t._sclm = sclm;
            return result;
        }

        public static async Task<StoryClient> GetClientAsync(this SCLM sclm, int clientId)
        {
            StoryClient client = await sclm.GETAsync<StoryClient>(new Uri($"{sclm.Endpoint}/{Version}/{PathClients}/{clientId}", UriKind.Absolute));
            client._sclm = sclm;
            return client;
        }

        public static async Task<StoryPresentation> GetPresentationAsync(this SCLM sclm, int presentationId)
        {
            StoryPresentation presentation = await sclm.GETAsync<StoryPresentation>(new Uri($"{sclm.Endpoint}/{Version}/{PathPresentations}/{presentationId}", UriKind.Absolute));
            presentation._sclm = sclm;
            return presentation;
        }

        public static async Task<StoryMediafile> GetMediafileAsync(this SCLM sclm, int mediafileId) =>
            await sclm.GETAsync<StoryMediafile>(new Uri($"{sclm.Endpoint}/{Version}/{PathMediafiles}/{mediafileId}", UriKind.Absolute));

        public static async Task<StorySlide> GetSlideAsync(this SCLM sclm, int slideId) =>
            await sclm.GETAsync<StorySlide>(new Uri($"{sclm.Endpoint}/{Version}/{PathSlides}/{slideId}", UriKind.Absolute));

        public static async Task<StoryPackageSas> GetContentPackageAsync(this SCLM sclm, int presentationId) =>
            await sclm.GETAsync<StoryPackageSas>(new Uri($"{sclm.Endpoint}/{Version}/{PathContentpackages}/{presentationId}", UriKind.Absolute));
    }
}
