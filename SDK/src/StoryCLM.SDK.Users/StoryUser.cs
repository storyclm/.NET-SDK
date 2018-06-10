using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public class StoryUser
    {
        public SCLM Context { get; set; }

        public StoryUser() { }

        Uri GetUri(string query = null) =>
            new Uri($"{Context.Endpoint}/{UsersExtensions.Version}/{UsersExtensions.Path}/{query}", UriKind.Absolute);

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        public async Task SaveAsync() =>
            await Context.PUTAsync<StoryUser>(GetUri(), this);

        public async Task ChangePasswordAsync(string password) =>
           await Context.PUTAsync(GetUri(Id + "/password"), new StoryPassword { Password = password });

        public async Task AddToGroupAsync(int groupId) =>
            await Context.PUTAsync(GetUri(Id + "/group/" + groupId), null);

        public async Task RemoveFromGroupAsync(int groupId) =>
            await Context.DELETEAsync(GetUri(Id + "/group/" + groupId));

        public async Task AddPresentationAsync(int presentationId) =>
            await Context.PUTAsync(GetUri(Id + "/presentation/" + presentationId), null);

        public async Task<IEnumerable<int>> AddPresentationsAsync(IEnumerable<int> ids) =>
            await Context.POSTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids);

        public async Task<IEnumerable<int>> SyncPresentationsAsync(IEnumerable<int> ids) =>
            await Context.PUTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids);

        public async Task RemoveFromPresentationAsync(int presentationId) =>
            await Context.DELETEAsync(GetUri(Id + "/presentation/" + presentationId));

        public async Task<IEnumerable<StorySimpleUserItem>> GetPresentations() =>
            await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/presentations"));

        public async Task<IEnumerable<StorySimpleUserItem>> GetGroups() =>
            await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/groups"));

    }
}
