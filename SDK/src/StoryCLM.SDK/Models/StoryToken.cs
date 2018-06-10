using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
   public class StoryToken
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? Expires { get; set; }

        public string TokenType { get; set; }
    }
}
