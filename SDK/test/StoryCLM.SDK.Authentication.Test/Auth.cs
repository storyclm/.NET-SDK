using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Authentication.Test
{
    public class Auth
    {
        [Fact]
        public async Task UserLogin()
        {
            SCLM sclm = new SCLM();
            StoryToken token = await sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, Settings.Username, Settings.Password);
            Assert.NotNull(token);
            Assert.NotNull(token.AccessToken);
            Assert.NotNull(token.RefreshToken);

            IEnumerable<object> objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);
            Assert.NotNull(objects);

            await Task.Delay(5000);
            objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);
            Assert.NotNull(objects);

            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, Settings.Username, null));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, null, Settings.Password));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, null, null));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, null, Settings.Username, Settings.Password));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret + "w", Settings.Username, Settings.Password));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId + "s", Settings.UserSecret, Settings.Username, Settings.Password));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(null, Settings.UserSecret, Settings.Username, Settings.Password));
        }

        [Fact]
        public async Task ServiceLogin()
        {
            SCLM sclm = new SCLM();
            StoryToken token = await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret);
            Assert.NotNull(token);
            Assert.NotNull(token.AccessToken);

            Assert.Null(token.RefreshToken);

            IEnumerable<object> objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);
            Assert.NotNull(objects);

            await Task.Delay(5000);
            objects = await sclm.GETAsync<IEnumerable<object>>(new Uri($"{sclm.GetEndpoint("api")}/v1/users/"), CancellationToken.None);
            Assert.NotNull(objects);

            await Assert.ThrowsAsync<Exception>(async () => await sclm.AuthAsync(Settings.ServiceClientId, null));
            await Assert.ThrowsAsync<Exception>(async () => await sclm.AuthAsync(null, Settings.ServiceSecret));
            await Assert.ThrowsAsync<Exception>(async () => await sclm.AuthAsync(Settings.ServiceClientId + "sdf", Settings.ServiceSecret));
            await Assert.ThrowsAsync<Exception>(async () => await sclm.AuthAsync(Settings.ServiceClientId, Settings.ServiceSecret + "sadf"));
            await Assert.ThrowsAsync<Exception>(async () => await sclm.AuthAsync(Settings.ServiceClientId + "sdf", Settings.ServiceSecret + "we"));
        }

        [Fact]
        public async Task Refresh()
        {
            SCLM sclm = new SCLM();
            StoryToken token = await sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, Settings.Username, Settings.Password);
            Assert.NotNull(token.RefreshToken);

            var token1 = await sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, token.RefreshToken);
            Assert.NotNull(token1.RefreshToken);

            var token2 = await sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, token1.RefreshToken);
            Assert.NotNull(token2.RefreshToken);

            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, token.RefreshToken));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, null));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, Settings.UserSecret, token.RefreshToken + "sdfsdf"));
            await Assert.ThrowsAsync<Exception>(() => sclm.AuthAsync(Settings.UserClientId, null, token2.RefreshToken));
        }
    }
}
