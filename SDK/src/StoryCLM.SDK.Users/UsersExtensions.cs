using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public static class UsersExtensions
    {
        public static string Version = "v1";
        public static string Path = @"users";

        public static async Task<StoryUser> UserExistsAsync(this SCLM sclm, string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            StoryUser user = await sclm.GETAsync<StoryUser>(new Uri($"{sclm.Endpoint}/{Version}/{Path}/exists?username={username}", UriKind.Absolute));
            user._sclm = sclm;
            return user;
        }

        public static async Task<StoryUser> CreateUserAsync(this SCLM sclm, StoryCreateUserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            StoryUser u = await sclm.POSTAsync<StoryUser>(new Uri($"{sclm.Endpoint}/{Version}/{Path}", UriKind.Absolute), user);
            u._sclm = sclm;
            return u;
        }

        public static async Task<StoryUser> GetUserAsync(this SCLM sclm, string userId)
        {
            StoryUser user = await sclm.GETAsync<StoryUser>(new Uri($"{sclm.Endpoint}/{Version}/{Path}/{userId}", UriKind.Absolute));
            user._sclm = sclm;
            return user;
        }

        public static async Task<IEnumerable<StoryUserItem>> GetUsersAsync(this SCLM sclm)
        {
            var users = await sclm.GETAsync<IEnumerable<StoryUserItem>>(new Uri($"{sclm.Endpoint}/{Version}/{Path}", UriKind.Absolute));
            foreach (var t in users) t._sclm = sclm;
            return users;
        }
    }
}
