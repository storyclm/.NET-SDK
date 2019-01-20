using Breffi.Story.Common.Retry;
using StoryCLM.SDK.Myosotis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Myosotis
{
    public static class MyosotisExtensions
    {
        const string _get = "GET";
        const string _put = "PUT";
        const string _apiVersionV2 = "api/v2/";

        static bool PetryPredicate(Exception ex)
        {
            if (ex == null) return false;
            if (new List<Type>() {
                    typeof(HttpRequestException),
                    typeof(TimeoutException),
                    typeof(IOException),
                    typeof(EndOfStreamException),
                    typeof(InvalidDataException),
                    typeof(InvalidOperationException),
                    typeof(TaskCanceledException)
                }.Any(t => ex.GetType() == t)) return true;

            HttpCommandException commandException = ex as HttpCommandException;
            if (commandException == null) return false;
            return new int[]
                {
                    502,
                    503,
                    504,
                    509,
                    520,
                    521
                }.Any(t => t == commandException.Code);
        }

        static async Task<T> SendAsync<T>(SCLM sclm, string method, Uri uri, object o, CancellationToken cancellationToken, IRetryPolicy retryPolicy = null) where T : class
        {
            using (MyosotisCommand<T> command = new MyosotisCommand<T>())
            {
                command.Method = method;
                command.Uri = uri;
                command.Data = o;
                await sclm.ExecuteHttpCommand(command, retryPolicy ?? new RetryPolicy()
                {
                    Predicate = (ex) => PetryPredicate(ex)
                }, cancellationToken, null);
                if (command.Exception != null) throw command.Exception;
                return command.Result;
            }
        }

        static Uri GetMyoEndpoint(this SCLM sclm) => sclm.GetEndpoint("myo");

        public static async Task<MyoProfile> GetProfileAsync(this SCLM sclm) =>
            await SendAsync<MyoProfile>(sclm, _get, new Uri($"{sclm.GetMyoEndpoint()}{_apiVersionV2}user/"), null, CancellationToken.None);

        public static async Task<MyoProfile> GetProfileAsync(this SCLM sclm, string id) =>
            await SendAsync<MyoProfile>(sclm, _get, new Uri($"{sclm.GetMyoEndpoint()}{_apiVersionV2}user/{id}/"), null, CancellationToken.None);

        public static async Task<MyoProfile> GetProfileByXmppLoginAsync(this SCLM sclm, string xmpplogin) =>
            await SendAsync<MyoProfile>(sclm, _get, new Uri($"{sclm.GetMyoEndpoint()}{_apiVersionV2}user/?xmpp={xmpplogin}"), null, CancellationToken.None);

        public static async Task<MyoProfile> UpdateProfileAsync(this SCLM sclm, MyoProfile profile) =>
            await SendAsync<MyoProfile>(sclm, _put, new Uri($"{sclm.GetMyoEndpoint()}{_apiVersionV2}user/{profile.UserId}/"), profile, CancellationToken.None);

        public static async Task<IEnumerable<MyoProfile>> GetContactsAsync(this SCLM sclm) =>
            await SendAsync<IEnumerable<MyoProfile>>(sclm, _get, new Uri($"{sclm.GetMyoEndpoint()}{_apiVersionV2}/user/contacts/"), null, CancellationToken.None);
    }
}
