using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.Content.Test
{
    public class Content
    {
        [Theory]
        [InlineData(0)]
        public async void GetClients(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var clients = await sclm.GetClientsAsync();
            Assert.True(clients.Count() > 0);
        }

        [Theory]
        [InlineData(0, 18)]
        public async void GetClient(int uc, int clientId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var client = await sclm.GetClientAsync(clientId);
            Assert.NotNull(client);
        }

        [Theory]
        [InlineData(0, 18)]
        public async void GetPresentationFromClient(int uc, int clientId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var client = await sclm.GetClientAsync(clientId);
            var pres = await client.Presentations.First().LoadAsync();
            Assert.NotNull(pres);
        }

        [Theory]
        [InlineData(0, 18)]
        public async void GetUserFromClient(int uc, int clientId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var client = await sclm.GetClientAsync(clientId);
            var pres = await client.Users.First().LoadAsync();
            Assert.NotNull(pres);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetPresentaionFromClient(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            Assert.NotNull(presentation);
        }

        [Theory]
        [InlineData(0, 4991)]
        public async void GetMediafilePresentaion(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            var mediafile = await presentation.MediaFiles.First().LoadAsync();
            Assert.NotNull(mediafile);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetUserFromPresentaion(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            var user = await presentation.Users.First().LoadAsync();
            Assert.NotNull(user);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetSlideFromPresentaion(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            var slide = await presentation.Slides.First().LoadAsync();
            Assert.NotNull(slide);
        }

        [Theory]
        [InlineData(0, 4991)]
        [InlineData(0, 5358)]
        public async void GetPackageFromPresentaion(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var presentation = await sclm.GetPresentationAsync(presentationId);
            var package = await presentation.SourcesFolder.LoadAsync();
            Assert.NotNull(package);
        }

        [Theory]
        [InlineData(0, 257754)]
        public async void GetSlide(int uc, int slideId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var result = await sclm.GetSlideAsync(slideId);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0, 10167)]
        public async void GetMediafile(int uc, int mediafileId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var result = await sclm.GetMediafileAsync(mediafileId);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0, 4991)]
        public async void GetPackage(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var result = await sclm.GetContentPackageAsync(presentationId);
            Assert.NotNull(result);
        }


        [Theory]
        [InlineData(0, 4991)]
        public async void UsersForPresentations(int uc, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            string[] usersIds = { "31e806b7-56b2-4560-ad39-2fa1a382a9d2", "d0532be9-d6d8-4401-8155-01e309f87aa7", "b2b68ca3-4e7b-4e36-b29e-dbcc06585065" };

            StoryPresentation presentation = await sclm.GetPresentationAsync(presentationId);
            Assert.NotNull(presentation);

            await presentation.RemoveAllUsersAsync();
            Assert.True(presentation.Users.Count() == 0);

            var users = await presentation.AddUsersAsync(usersIds);
            Assert.True(presentation.Users.Count() == 3);

            users = await presentation.AddUsersAsync(usersIds);
            Assert.True(presentation.Users.Count() == 3);


        }

    }
}
