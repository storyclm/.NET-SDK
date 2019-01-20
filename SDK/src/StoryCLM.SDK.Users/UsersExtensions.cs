using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public static class UsersExtensions
    {
        public static string Version = "v1";
        public static string Path = @"users";
        const string api = nameof(api);

        public static async Task<StoryUser> UserExistsAsync(this SCLM sclm, string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            StoryUser user = await sclm.GETAsync<StoryUser>(new Uri($"{sclm.GetEndpoint(api)}{Version}/{Path}/exists?username={username}", UriKind.Absolute), CancellationToken.None);
            user.Context = sclm;
            return user;
        }

        public static async Task<StoryUser> CreateUserAsync(this SCLM sclm, StoryCreateUserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            StoryUser u = await sclm.POSTAsync<StoryUser>(new Uri($"{sclm.GetEndpoint(api)}/{Version}/{Path}", UriKind.Absolute), user, CancellationToken.None);
            u.Context = sclm;
            return u;
        }

        public static async Task<StoryUser> GetUserAsync(this SCLM sclm, string userId)
        {
            StoryUser user = await sclm.GETAsync<StoryUser>(new Uri($"{sclm.GetEndpoint(api)}/{Version}/{Path}/{userId}", UriKind.Absolute), CancellationToken.None);
            user.Context = sclm;
            return user;
        }

        public static async Task<StoryUser> GetUserAsync(this SCLM sclm, StoryUserItem user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            StoryUser u = await sclm.GETAsync<StoryUser>(new Uri($"{sclm.GetEndpoint(api)}/{Version}/{Path}/{user.Id}", UriKind.Absolute), CancellationToken.None);
            u.Context = sclm;
            return u;
        }

        public static async Task<IEnumerable<StoryUserItem>> GetUsersAsync(this SCLM sclm)
        {
            var users = await sclm.GETAsync<IEnumerable<StoryUserItem>>(new Uri($"{sclm.GetEndpoint(api)}/{Version}/{Path}", UriKind.Absolute), CancellationToken.None);
            foreach (var t in users) t.Context = sclm;
            return users;
        }
    }
}
