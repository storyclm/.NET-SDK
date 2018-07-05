using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public class StoryUser
    {
        public SCLM Context { get; set; }

        public StoryUser() { }

        Uri GetUri(string query = null) =>
            new Uri($"{Context.GetEndpoint("api")}{UsersExtensions.Version}/{UsersExtensions.Path}/{query}", UriKind.Absolute);

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        public async Task SaveAsync() =>
            await Context.PUTAsync<StoryUser>(GetUri(), this, CancellationToken.None);

        public async Task ChangePasswordAsync(string password) =>
           await Context.PUTAsync<Object>(GetUri(Id + "/password"), new StoryPassword { Password = password }, CancellationToken.None);

        public async Task AddToGroupAsync(int groupId) =>
            await Context.PUTAsync<Object>(GetUri(Id + "/group/" + groupId), null, CancellationToken.None);

        public async Task RemoveFromGroupAsync(int groupId) =>
            await Context.DELETEAsync<Object>(GetUri(Id + "/group/" + groupId), CancellationToken.None);

        public async Task AddPresentationAsync(int presentationId) =>
            await Context.PUTAsync<Object>(GetUri(Id + "/presentation/" + presentationId), null, CancellationToken.None);

        public async Task<IEnumerable<int>> AddPresentationsAsync(IEnumerable<int> ids) =>
            await Context.POSTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids, CancellationToken.None);

        public async Task<IEnumerable<int>> SyncPresentationsAsync(IEnumerable<int> ids) =>
            await Context.PUTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids, CancellationToken.None);

        public async Task RemoveFromPresentationAsync(int presentationId) =>
            await Context.DELETEAsync<Object>(GetUri(Id + "/presentation/" + presentationId), CancellationToken.None);

        public async Task<IEnumerable<StorySimpleUserItem>> GetPresentations() =>
            await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/presentations"), CancellationToken.None);

        public async Task<IEnumerable<StorySimpleUserItem>> GetGroups() =>
            await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/groups"), CancellationToken.None);

    }
}
