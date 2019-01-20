using System;

namespace StoryCLM.SDK.Authentication
{
    public class StoryToken
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? Expires { get; set; }

        public string TokenType { get; set; }
    }
}
