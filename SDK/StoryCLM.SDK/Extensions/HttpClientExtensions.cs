using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Extensions
{
   public static class HttpClientExtensions
    {
        public static void SetToken(this HttpClient client, StoryToken token)
        {
            if (token == null) return;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token?.AccessToken ?? string.Empty);
        }
    }
}
