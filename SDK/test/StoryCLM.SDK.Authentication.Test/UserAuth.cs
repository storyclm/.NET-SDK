using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Authentication.Test
{
    public class UserAuth
    {
        string client = "client_18_4";
        string secret = "1cdbbf4374634314bfd5607a79a0b5578d05130732dc4a37ac8c046525a27075";
        string password = "4JwGce";
        string username = "rsk-k161@ya.ru";

        [Fact]
        public async Task Login()
        {
            SCLM sclm = new SCLM();
            StoryToken token = await sclm.AuthAsync(client, secret, username, password);
            Assert.NotNull(token);
            Assert.NotNull(token.AccessToken);
            Assert.NotNull(token.RefreshToken);

            IEnumerable<object> objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);

        }
    }
}
