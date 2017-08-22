using StoryCLM.SDK;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
   public class Users
    {
        [Theory]
        [InlineData("test@test.com", 4991, 87)]
        //[InlineData("test@test.com", 4962, 64)]
        public async void User(string username, int presentationId, int groupId)
        {
            SCLM sclm = await Utilities.GetContextAsync();
            StoryCreateUserModel userForCreate = new StoryCreateUserModel() {
                Username = username,
                Email = "test@test.com",
                Phone = "79190000000",
                BirthDate = DateTime.Now,
                Gender = true,
                Location = "Stavropol",
                Name = "Testname",
                Password = "1234"
            };

            StoryUser user = await sclm.CreateUserAsync(userForCreate);
            Assert.NotNull(user);
            user = await sclm.UserExistsAsync(user.Username);
            Assert.NotNull(user);
            user.Name = "SuperTestuser";
            await user.SaveAsync();
            user = await sclm.GetUserAsync(user.Id);
            Assert.NotNull(user);
            Assert.True(user.Name == "SuperTestuser");
            await user.ChangePasswordAsync("1234");

            await user.AddToPresentationAsync(presentationId);
            var ps = await user.GetPresentations();
            Assert.True(ps.Select(t => t.Id).Contains(presentationId));
            await user.RemoveFromPresentationAsync(presentationId);
            ps = await user.GetPresentations();
            Assert.False(ps.Select(t => t.Id).Contains(presentationId));

            await user.AddToGroupAsync(groupId);
            var gs = await user.GetGroups();
            Assert.True(gs.Select(t => t.Id).Contains(groupId));
            await user.RemoveFromGroupAsync(groupId);
            gs = await user.GetGroups();
            Assert.False(gs.Select(t => t.Id).Contains(groupId));

            var usrs = await sclm.GetUsersAsync();

        }
    }
}
