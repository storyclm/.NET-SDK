using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
   public class StoryUserItem: StorySimpleUser
    {
        internal SCLM _sclm;

        internal StoryUserItem() { }

        public string Username { get; set; }

        public async Task<StoryUser> GetUserAsync()
        {
            StoryUser user = await _sclm.GETAsync<StoryUser>(_sclm.kUsers + Id, string.Empty);
            user._sclm = _sclm;
            return user;
        }

    }
}
