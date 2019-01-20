using Shared;
using System;
using System.Linq;
using Xunit;

namespace StoryCLM.SDK.Users.Test
{
    public class Users
    {
        static Settings Settings => Settings.Get();

        public StoryCreateUserModel GetUserModel(string username)
        {
            return new StoryCreateUserModel()
            {
                Username = username,
                Email = "test@test.com",
                Phone = "79190000000",
                BirthDate = DateTime.Now,
                Gender = true,
                Location = "Stavropol",
                Name = "Testname",
                Password = "1234"
            };
        }

        public StoryCreateUserModel GetUserModel2(string username)
        {
            return new StoryCreateUserModel()
            {
                Username = username,
                Email = null,
                Phone = null,
                BirthDate = DateTime.Now,
                Gender = true,
                Location = "Stavropol",
                Name = "",
                Password = "1234"
            };
        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void Create(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            StoryCreateUserModel userForCreate = GetUserModel2(username);
            StoryUser user = await sclm.CreateUserAsync(userForCreate);
            Assert.NotNull(user);
        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void Exists(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            var user = await sclm.UserExistsAsync(username);
            Assert.NotNull(user);
        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void Login(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            SCLM s = new SCLM();
            await s.AuthAsync(Settings.UserClientId, Settings.UserSecret, username, Settings.Password);
        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void Update(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            user.Name = "SuperTestuser";
            await user.SaveAsync();
            user = await sclm.GetUserAsync(user.Id);
            Assert.NotNull(user);
            Assert.True(user.Name == "SuperTestuser");

        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void ChangePassword(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.ChangePasswordAsync("test");
            await user.ChangePasswordAsync("1234");
        }

        [Theory]
        [InlineData(0, "test@test.com", 4991)]
        public async void AddPresentation(int uc, string username, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.AddPresentationAsync(presentationId);
            var ps = await user.LoadPresentations();
            Assert.Contains(presentationId, ps.Select(t => t.Id));
        }

        [Theory]
        [InlineData(0, "test@test.com", 4991)]
        public async void RemovePresentations(int uc, string username, int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.RemoveFromPresentationAsync(presentationId);
            var ps = await user.LoadPresentations();
            Assert.DoesNotContain(presentationId, ps.Select(t => t.Id));
        }

        [Theory]
        [InlineData(0, "test@test.com", 87)]
        public async void AddToGroups(int uc, string username, int groupId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.AddToGroupAsync(groupId);
            var gs = await user.LoadGroups();
            Assert.Contains(groupId, gs.Select(t => t.Id));
        }

        [Theory]
        [InlineData(0, "test@test.com", 87)]
        public async void GetGroups(int uc, string username, int groupId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.AddToGroupAsync(groupId);
            await user.LoadGroups();
            Assert.Contains(groupId, user.Groups.Select(t => t.Id));
        }

        [Theory]
        [InlineData(0, "test@test.com", 87)]
        public async void RemoveFromGroups(int uc, string username, int groupId)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            await user.RemoveFromGroupAsync(groupId);
            var gs = await user.LoadGroups();
            Assert.DoesNotContain(groupId, gs.Select(t => t.Id));
        }

        [Theory]
        [InlineData(0, "test@test.com")]
        public async void AllUsers(int uc, string username)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var user = await sclm.UserExistsAsync(username);

            var usrs = await sclm.GetUsersAsync();
            Assert.True(usrs.Count() > 0);
        }
    }
}
