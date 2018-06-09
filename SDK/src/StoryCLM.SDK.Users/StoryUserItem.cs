using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
   public class StoryUserItem: StorySimpleUser
    {
        internal SCLM _sclm;

        internal StoryUserItem() { }

        public string Username { get; set; }

        public async Task<StoryUser> GetUserAsync()
        {
            StoryUser user = await _sclm.GETAsync<StoryUser>(new Uri($"{_sclm.Endpoint}/{UsersExtensions.Version}/{UsersExtensions.Path}/{Id}", UriKind.Absolute));
            user._sclm = _sclm;
            return user;
        }

    }
}
