using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
   public class StoryUserItem: StorySimpleUser
    {
        internal StoryUserItem() { }

        public string Username { get; set; }
    }
}
